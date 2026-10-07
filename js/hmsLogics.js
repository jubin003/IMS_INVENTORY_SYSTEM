// TABLE SORTING
const sortDirections = {};
/*
    Create a unique sessionStorage key for the current page.
    Example:
    /imsv5_mbcg/hms/building.aspx

*/
function getTableSortStorageKey() {
    return "hmsTableSort:" + window.location.pathname.toLowerCase();
}


/*
    Called when clicking a sortable column header.
*/
function sortTableColumn(headerElement, targetSelector) {
    if (!headerElement || !targetSelector) {
        return;
    }

    // Find table containing clicked header
    const table = headerElement.closest("table");

    if (!table) { return; }

    // Unique sort key for table + selected column
    const tableKey = table.id || "table";

    const sortKey = tableKey + "_" + targetSelector;

    /*
        First click = ascending
        Second click = descending
    */
    const isAscending = sortDirections[sortKey] === undefined ? true : !sortDirections[sortKey];

    sortDirections[sortKey] = isAscending;

    // Perform sorting
    applyTableSort( table, headerElement, targetSelector, isAscending );

    // Save state for this current page only
    sessionStorage.setItem( getTableSortStorageKey(), JSON.stringify({targetSelector: targetSelector, ascending: isAscending})
    );
}


/*
    Actually sorts complete table rows.
*/
function applyTableSort( table, headerElement, targetSelector, isAscending) {
    if (!table) { return; }

    const allRows =
        Array.from(table.rows);

    /*
        Only rows containing the selected sortable element
        are treated as data rows.

        This automatically excludes:
        - header rows
        - pager rows
        - footer rows
    */
    const dataRows = allRows.filter(function (row) {

            return row.querySelector(
                targetSelector
            ) !== null;

        });


    if (dataRows.length === 0) { return; }


    // FIND TRAILING PAGER / FOOTER
    const lastDataRowIndex = Math.max.apply(null, dataRows.map(function (row) {
                return row.rowIndex;
            })
        );


    const anchorRow = allRows.find(function (row) {
            return row.rowIndex >
                lastDataRowIndex;
        });


    // SORT COMPLETE ROWS
    dataRows.sort(function (rowA, rowB) {

        const elementA = rowA.querySelector(
                targetSelector
            );


        const elementB = rowB.querySelector(
                targetSelector
            );


        const valueA = elementA ? elementA.textContent.trim() : "";


        const valueB = elementB ? elementB.textContent.trim() : "";

        const comparison = valueA.localeCompare( valueB, undefined,
                {
                    numeric: true,
                    sensitivity: "base"
                }
            );


        return isAscending ? comparison : -comparison;
    });

    // REINSERT COMPLETE ROWS
    const container = dataRows[0].parentNode;


    dataRows.forEach(function (row) {

        if (anchorRow) {
            container.insertBefore( row, anchorRow );
        }
        else {
            container.appendChild( row );
        }

    });


    // RESET ALL SORT HEADERS
    const headers = table.querySelectorAll( ".sortable-header" );
    headers.forEach(function (header) {

        header.classList.remove( "sort-ascending", "sort-descending" );
        const icon = header.querySelector( ".sort-icon" );
        if (icon) {
            icon.style.display = "none";

            icon.classList.remove(
                "fa-chevron-up",
                "fa-chevron-down",
                "fa-sort-asc",
                "fa-sort-desc"
            );
        }
    });


    // ACTIVATE CURRENT HEADER
    if (!headerElement) { return; }


    headerElement.classList.add( isAscending ? "sort-ascending" : "sort-descending" );


    const activeIcon = headerElement.querySelector( ".sort-icon" );


    if (activeIcon) {
        activeIcon.style.display = "inline-block";
        activeIcon.classList.add( isAscending ? "fa-chevron-up" : "fa-chevron-down" );
    }
}

function restoreTableSort() {
    const storageKey = getTableSortStorageKey();
    const savedSort = sessionStorage.getItem( storageKey );
    if (!savedSort) { return; }
    let state;
    try {
        state = JSON.parse(savedSort);

    }
    catch (error) {
        sessionStorage.removeItem(
            storageKey
        );
        return;
    }


    if (
        !state ||
        !state.targetSelector
    ) {
        return;
    }


    /*
        Find a row containing the selector.
        Example:
        .sort-sn
    */
    const sortableElement =
        document.querySelector(
            state.targetSelector
        );


    if (!sortableElement) {
        return;
    }


    const table =
        sortableElement.closest(
            "table"
        );


    if (!table) {
        return;
    }


    // Find matching clickable header
    const headers =
        Array.from(
            table.querySelectorAll(
                ".sortable-header"
            )
        );


    const header =
        headers.find(function (item) {

            const onclick =
                item.getAttribute(
                    "onclick"
                ) || "";


            return onclick.indexOf(
                state.targetSelector
            ) !== -1;

        });


    if (!header) {
        return;
    }


    // Reapply saved sorting
    applyTableSort(
        table,
        header,
        state.targetSelector,
        state.ascending
    );


    /*
        Restore toggle state.

        Otherwise if ASC was restored,
        next click might incorrectly start from ASC again.
    */
    const tableKey =
        table.id || "table";


    const sortKey =
        tableKey +
        "_" +
        state.targetSelector;


    sortDirections[sortKey] =
        state.ascending;
}


// CLEAR SORT WHEN NAVIGATING TO ANOTHER PAGE
function setupSortNavigationCleanup() {

    $(document).on(
        "click",
        "a[href]",
        function () {

            const href =
                $(this).attr(
                    "href"
                );


            // Ignore non-navigation links
            if (
                !href ||
                href === "#" ||
                href.toLowerCase()
                    .indexOf("javascript:") === 0
            ) {
                return;
            }


            // Ignore links opening in another tab
            if (
                $(this).attr(
                    "target"
                ) === "_blank"
            ) {
                return;
            }


            let targetUrl;


            try {

                targetUrl =
                    new URL(
                        this.href,
                        window.location.origin
                    );

            }
            catch (error) {

                return;
            }


            const currentPath =
                window.location.pathname
                    .toLowerCase()
                    .replace(/\/$/, "");


            const targetPath =
                targetUrl.pathname
                    .toLowerCase()
                    .replace(/\/$/, "");


            /*
                Only clear if actually going
                to another page.
            */
            if (
                targetPath !== currentPath
            ) {

                sessionStorage.removeItem(
                    getTableSortStorageKey()
                );

            }

        }
    );
}


// ============================================================
// GENERIC GRID SEARCH
// ============================================================

/*
    Search is completely independent from sorting.

    Example:

    onkeyup="filterGridTable(this, 1)"

    columnIndex:
    0 = first column
    1 = second column
    2 = third column
*/
function filterGridTable(
    inputElement,
    columnIndex
) {

    if (!inputElement) {
        return;
    }


    const filter =
        inputElement.value
            .toLowerCase()
            .trim();


    // Find nearest card/container
    const container =
        inputElement.closest(
            ".bs-card"
        );


    if (!container) {
        return;
    }


    const table =
        container.querySelector(
            "table.enterprise-grid"
        );


    if (!table) {
        return;
    }


    const rows =
        table.querySelectorAll(
            "tr"
        );


    /*
        Start from 1 because row 0
        is normally the header.
    */
    for (
        let i = 1;
        i < rows.length;
        i++
    ) {

        const row =
            rows[i];


        // Ignore pager
        if (
            row.classList.contains(
                "gridview-pager"
            ) ||
            row.querySelector(
                ".gridview-pager"
            )
        ) {
            continue;
        }


        const cells =
            row.getElementsByTagName(
                "td"
            );


        if (!cells[columnIndex]) {
            continue;
        }


        const value =
            (
                cells[columnIndex]
                    .textContent ||
                cells[columnIndex]
                    .innerText ||
                ""
            )
            .toLowerCase();


        row.style.display =
            value.indexOf(filter) !== -1
                ? ""
                : "none";

    }
}


// PAGE READY
$(document).ready(function () {

    /*
        Restore current page sorting after
        WebForms has rendered the GridView.
    */
    restoreTableSort();

    /*
        Setup link navigation cleanup.
    */
    setupSortNavigationCleanup();

});