<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Load Chart.js -->
    <style>
        /* Toggle Switch Container */
        .toggle-container {
            display: flex;
            align-items: center;
            gap: 10px;
        }



        /* Toggle Switch */
        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 25px;
        }

            /* Hide Default Checkbox */
            .switch input {
                display: none;
            }

        /* Slider */
        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            transition: 0.4s;
            border-radius: 25px;
        }

            /* Round Slider */
            .slider.round:before {
                content: "";
                position: absolute;
                width: 20px;
                height: 20px;
                left: 5px;
                bottom: 2.5px;
                background-color: white;
                border-radius: 50%;
                transition: 0.4s;
            }

        /* When Checked */
        .switch input:checked + .slider {
            background-color: #2196F3;
        }

            /* Move Toggle */
            .switch input:checked + .slider:before {
                transform: translateX(25px);
            }
    </style>


    <style>
        .hidden-grid {
            display: none;
        }
    </style>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <!-- Top Sales Chart Script -->
    <script type="text/javascript">
        function renderTopSalesChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }

            console.log(data); // Log data for debugging

            var productLabels = [];
            var salesValues = [];

            data.forEach(function (item) {
                productLabels.push(item.PRODUCT_NAME);
                salesValues.push(parseFloat(item.TOTAL_AMOUNT_AFTER_TAX));
            });

            var ctx = document.getElementById('chartTopSalesIn30Canvas').getContext('2d');
            new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: productLabels,
                    datasets: [{
                        label: 'Total Sales',
                        data: salesValues,
                        backgroundColor: [
                            '#FF6384', '#36A2EB', '#FFCE56', '#66ff66', '#ff9933', '#c44dff',
                            '#8A2BE2', '#FF4500', '#00FA9A', '#FFD700', '#FF1493', '#20B2AA'
                        ],
                        hoverBackgroundColor: [
                            '#FF80A5', '#5AB8F5', '#FFE084', '#80ff80', '#ffb366', '#d580ff',
                            '#9B30FF', '#FF6347', '#3CB371', '#FFE135', '#FF69B4', '#40E0D0'
                        ],
                        hoverOffset: 4
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: { display: false }, // Hide the legend
                        title: { display: true, text: 'Top 6 Items Sold in 30 Days' }
                    }
                }
            });
        }
    </script>
    <!-- Daily Sales Chart Script -->
    <script type="text/javascript">
        function renderSalesChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }

            console.log(" Sales Data:", data); // Debugging
            var salesValue = data.length > 0 && data[0].INVOICE_AMOUNT ? parseFloat(data[0].INVOICE_AMOUNT) : 0; // Get the sales value
            var ctx = document.getElementById('chartDailySalesCanvas').getContext('2d');

            // Center text plugin for cash sales
            const cashCenterTextPlugin = {
                id: 'cashCenterText',
                afterDraw: function (chart) {
                    var ctx = chart.ctx;
                    var centerX = (chart.chartArea.left + chart.chartArea.right) / 2;
                    var centerY = (chart.chartArea.top + chart.chartArea.bottom) / 2;

                    ctx.save();
                    ctx.font = "12px Arial"; // Decrease font size
                    ctx.textBaseline = "middle";
                    ctx.textAlign = "center";
                    ctx.fillStyle = "#000"; // Text color

                    // Display cash sales text and value on two rows
                    ctx.fillText("Daily Sales", centerX, centerY - 10);
                    ctx.fillText(salesValue.toFixed(2), centerX, centerY + 5); // Adjust vertical position
                    ctx.restore();
                }
            };


            // Create the chart and register the plugin only for this instance
            new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Total Sales'],
                    datasets: [{
                        label: 'Sales Amount',
                        data: ['1'],
                        backgroundColor: ['#36A2EB'],
                        hoverBackgroundColor: ['#39e75f'],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true, cutout: '80%',
                    plugins: {
                        legend: { display: false },
                        title: { display: true, text: 'Daily Sales Total' },
                        tooltip: { enabled: false }, // Enable tooltip to show on hover
                        cashCenterTextPlugin: { // Register the center text plugin
                            id: 'cashCenterTextPlugin'
                        }
                    }
                },
                plugins: [cashCenterTextPlugin] // Attach the plugin to this specific chart
            });
        }
    </script>
    <!-- Daily Cash Sales Chart Script -->
    <script type="text/javascript">
        function renderCashSalesChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }

            console.log("Cash Sales Data:", data); // Debugging
            var salesValue = data.length > 0 && data[0].INVOICE_AMOUNT ? parseFloat(data[0].INVOICE_AMOUNT) : 0; // Get the sales value
            var ctx = document.getElementById('chartCashDailySalesCanvas').getContext('2d');

            // Center text plugin for cash sales
            const cashCenterTextPlugin = {
                id: 'cashCenterText',
                afterDraw: function (chart) {
                    var ctx = chart.ctx;
                    var centerX = (chart.chartArea.left + chart.chartArea.right) / 2;
                    var centerY = (chart.chartArea.top + chart.chartArea.bottom) / 2;

                    ctx.save();
                    ctx.font = "12px Arial"; // Decrease font size
                    ctx.textBaseline = "middle";
                    ctx.textAlign = "center";
                    ctx.fillStyle = "#000"; // Text color

                    // Display cash sales text and value on two rows
                    ctx.fillText("Cash Sales", centerX, centerY - 10);
                    ctx.fillText(salesValue.toFixed(2), centerX, centerY + 5); // Adjust vertical position
                    ctx.restore();
                }
            };


            // Create the chart and register the plugin only for this instance
            new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Total Cash Sales'],
                    datasets: [{
                        label: 'Sales Amount',
                        data: ['1'],
                        backgroundColor: ['#66ff66'],
                        hoverBackgroundColor: ['#39e75f'],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true, cutout: '80%',
                    plugins: {
                        legend: { display: false },
                        title: { display: true, text: 'Daily Cash Sales Total' },
                        tooltip: { enabled: false }, // Enable tooltip to show on hover
                        cashCenterTextPlugin: { // Register the center text plugin
                            id: 'cashCenterTextPlugin'
                        }
                    }
                },
                plugins: [cashCenterTextPlugin] // Attach the plugin to this specific chart
            });
        }
    </script>
    <!-- Daily Credit Sales Chart Script -->
    <script type="text/javascript">
        function renderCreditSalesChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }

            console.log("Credit Sales Data:", data); // Debugging

            var creditSalesValue = data.length > 0 && data[0].INVOICE_AMOUNT ? parseFloat(data[0].INVOICE_AMOUNT) : 0; // Get the credit sales value
            var ctx = document.getElementById('chartDailyCreditSalesCanvas').getContext('2d');

            // Center text plugin for credit sales
            const creditCenterTextPlugin = {
                id: 'creditCenterText',
                afterDraw: function (chart) {
                    var ctx = chart.ctx;
                    var centerX = (chart.chartArea.left + chart.chartArea.right) / 2;
                    var centerY = (chart.chartArea.top + chart.chartArea.bottom) / 2;

                    ctx.save();
                    ctx.font = "12px Arial"; // Decrease font size
                    ctx.textBaseline = "middle";
                    ctx.textAlign = "center";
                    ctx.fillStyle = "#000"; // Text color

                    // Display credit sales text and value on two rows
                    ctx.fillText("Credit Sales", centerX, centerY - 10);
                    ctx.fillText(creditSalesValue.toFixed(2), centerX, centerY + 5); // Adjust vertical position
                    ctx.restore();
                }
            };

            // Create the chart and register the plugin only for this instance
            new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Total Credit Sales'],
                    datasets: [{
                        label: 'Credit Sales Amount',
                        data: ['1'],
                        backgroundColor: ['#FF69B4'],
                        hoverBackgroundColor: ['#FF80A5'],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    cutout: '80%', // Increase this percentage to make the donut ring slimmer
                    plugins: {
                        legend: { display: false },
                        title: { display: true, text: 'Daily Credit Sales Total' },
                        tooltip: { enabled: false }, // Enable tooltip to show on hover
                        creditCenterTextPlugin: { // Register the center text plugin
                            id: 'creditCenterTextPlugin'
                        }
                    }
                },
                plugins: [creditCenterTextPlugin] // Attach the plugin to this specific chart
            });
        }
    </script>


    <!-- Daily Cash Purchase Chart Script -->
    <script type="text/javascript">
        function renderPurchasesChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }

            console.log("Purchases Data:", data); // Debugging

            var purchasesValue = data.length > 0 && data[0].INVOICE_AMOUNT ? parseFloat(data[0].INVOICE_AMOUNT) : 0; // Get the purchases value
            var ctx = document.getElementById('chartDailyPurchasesCanvas').getContext('2d');

            // Center text plugin for cash purchases
            const PurchasesCenterTextPlugin = {
                id: 'PurchasesCenterText',
                afterDraw: function (chart) {
                    var ctx = chart.ctx;
                    var centerX = (chart.chartArea.left + chart.chartArea.right) / 2;
                    var centerY = (chart.chartArea.top + chart.chartArea.bottom) / 2;

                    ctx.save();
                    ctx.font = "11px Arial"; // Decrease font size
                    ctx.textBaseline = "middle";
                    ctx.textAlign = "center";
                    ctx.fillStyle = "#000"; // Text color

                    // Display cash purchases text and value on two rows
                    ctx.fillText("Daily Purchases", centerX, centerY - 10);
                    ctx.fillText(purchasesValue.toFixed(2), centerX, centerY + 5); // Adjust vertical position
                    ctx.restore();
                }
            };

            // Create the chart and register the plugin
            new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Total Purchases'],
                    datasets: [{
                        label: 'Purchases Amount',
                        data: ['1'],
                        backgroundColor: ['#69B455'], // Same color as cash sales
                        hoverBackgroundColor: ['#F08080'],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true, cutout: '80%',
                    plugins: {
                        legend: { display: false },
                        title: { display: true, text: 'Daily Purchases Total' },
                        tooltip: { enabled: false }, // Enable tooltip to show on hover
                    }
                },
                plugins: [PurchasesCenterTextPlugin] // Attach the plugin to this specific chart
            });
        }
    </script>
    <!-- Daily Cash Purchase Chart Script -->
    <script type="text/javascript">
        function renderCashPurchasesChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }

            console.log("Cash Purchases Data:", data); // Debugging

            var purchasesValue = data.length > 0 && data[0].INVOICE_AMOUNT ? parseFloat(data[0].INVOICE_AMOUNT) : 0; // Get the purchases value
            var ctx = document.getElementById('chartCashDailyPurchasesCanvas').getContext('2d');

            // Center text plugin for cash purchases
            const cashPurchasesCenterTextPlugin = {
                id: 'cashPurchasesCenterText',
                afterDraw: function (chart) {
                    var ctx = chart.ctx;
                    var centerX = (chart.chartArea.left + chart.chartArea.right) / 2;
                    var centerY = (chart.chartArea.top + chart.chartArea.bottom) / 2;

                    ctx.save();
                    ctx.font = "11px Arial"; // Decrease font size
                    ctx.textBaseline = "middle";
                    ctx.textAlign = "center";
                    ctx.fillStyle = "#000"; // Text color

                    // Display cash purchases text and value on two rows
                    ctx.fillText("Cash Purchases", centerX, centerY - 10);
                    ctx.fillText(purchasesValue.toFixed(2), centerX, centerY + 5); // Adjust vertical position
                    ctx.restore();
                }
            };

            // Create the chart and register the plugin
            new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Total Cash Purchases'],
                    datasets: [{
                        label: 'Purchases Amount',
                        data: ['1'],
                        backgroundColor: ['#ffff66'], // Same color as cash sales
                        hoverBackgroundColor: ['#F08080'],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true, cutout: '80%',
                    plugins: {
                        legend: { display: false },
                        title: { display: true, text: 'Daily Cash Purchases Total' },
                        tooltip: { enabled: false }, // Enable tooltip to show on hover
                    }
                },
                plugins: [cashPurchasesCenterTextPlugin] // Attach the plugin to this specific chart
            });
        }
    </script>
    <!-- Daily Credit Purchase Chart Script -->
    <script type="text/javascript">
        function renderCreditPurchasesChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }
            console.log("Credit Purchases Data:", data); // Debugging

            var creditPurchasesValue = data.length > 0 && data[0].INVOICE_AMOUNT ? parseFloat(data[0].INVOICE_AMOUNT) : 0; // Get the credit purchases value
            var ctx = document.getElementById('chartDailyCreditPurchasesCanvas').getContext('2d');

            // Center text plugin for credit purchases
            const creditPurchasesCenterTextPlugin = {
                id: 'creditPurchasesCenterText',
                afterDraw: function (chart) {
                    var ctx = chart.ctx;
                    var centerX = (chart.chartArea.left + chart.chartArea.right) / 2;
                    var centerY = (chart.chartArea.top + chart.chartArea.bottom) / 2;

                    ctx.save();
                    ctx.font = "11px Arial"; // Decrease font size
                    ctx.textBaseline = "middle";
                    ctx.textAlign = "center";
                    ctx.fillStyle = "#000"; // Text color

                    // Display credit purchases text and value on two rows
                    ctx.fillText("Credit Purchases", centerX, centerY - 10);
                    ctx.fillText(creditPurchasesValue.toFixed(2), centerX, centerY + 5); // Adjust vertical position
                    ctx.restore();
                }
            };

            // Create the chart and register the plugin
            new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Total Credit Purchases'],
                    datasets: [{
                        label: 'Credit Purchases Amount',
                        data: ['1'],
                        backgroundColor: ['#d580ff'], // Same color as credit sales
                        hoverBackgroundColor: ['#000080'],
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true, cutout: '80%',
                    plugins: {
                        legend: { display: false },
                        title: { display: true, text: 'Daily Credit Purchases Total' },
                        tooltip: { enabled: false }, // Enable tooltip to show on hover
                    }
                },
                plugins: [creditPurchasesCenterTextPlugin] // Attach the plugin to this specific chart
            });
        }
    </script>

    <!-- Operation Cost Chart Script -->
    <script type="text/javascript">
        function renderOperatingCostLastSix(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }

            const labels = data.length > 1 ? [] : ["Last Six Month"];  // Default label if only one entry
            const debitAmounts = [];
            const creditAmounts = [];

            data.forEach(item => {
                // Push placeholder label if none is available
                labels.push(item.Month || "Last Six Month");  // Adjust to add month data if available
                debitAmounts.push(parseFloat(item.DEBIT));
                creditAmounts.push(parseFloat(item.CREDIT));
            });

            const ctx = document.getElementById('operatingCostLastSixChart').getContext('2d');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Total Debit Amount',
                            data: debitAmounts,
                            backgroundColor: '#FF6384',
                        },
                        {
                            label: 'Total Credit Amount',
                            data: creditAmounts,
                            backgroundColor: '#36A2EB',
                        }
                    ]
                },
                options: {

                    responsive: true, cutout: '80%',
                    plugins: {
                        legend: { display: false },
                        title: { display: true, text: 'Operating Costs (Last 6 Months)' }
                    },
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        }
    </script>
    <!-- Cashflow Chart Script -->
    <script type="text/javascript">
        function renderCashFlowChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }

            console.log("Cash Flow Data:", data); // Debugging

            var thisMonthSales = parseFloat(data[0].CURRENT_MONTH_CASH_SALES);
            var thisMonthPurchases = parseFloat(data[0].CURRENT_MONTH_CASH_PURCHASES);
            var lastMonthSales = parseFloat(data[0].PREVIOUS_MONTH_CASH_SALES);
            var lastMonthPurchases = parseFloat(data[0].PREVIOUS_MONTH_CASH_PURCHASES);

            var ctx = document.getElementById('cashFlowChartCanvas').getContext('2d');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ['This Month Sales', 'This Month Purchases', 'Last Month Sales', 'Last Month Purchases'],
                    datasets: [{
                        label: 'Cash Flow Amount',
                        data: [thisMonthSales, thisMonthPurchases, lastMonthSales, lastMonthPurchases],
                        backgroundColor: [
                            '#FF6384',
                            '#36A2EB',
                            '#FFCE56',
                            '#66ff66'
                        ],
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: { display: false },
                        title: { display: true, text: 'Cash Flow (Last 2 Months)' }
                    },
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        }
    </script>

    <!-- Top receiveable Chart Script -->
    <script type="text/javascript">
        function renderTopCreditorsChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }
            var labels = data.map(item => item.SUB_GL_NAME);
            var values = data.map(item => parseFloat(item.BALANCE));

            var ctx = document.getElementById('topCreditorsChart').getContext('2d');
            new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        data: values,
                        backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF'],
                        hoverBackgroundColor: ['#FF80A5', '#66B3FF', '#FFD666', '#66FFDC', '#BBAAFF']
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: { display: false, position: 'top' },
                        title: { display: true, text: 'Top 5 Receivables' }
                    }
                }
            });
        }
    </script>
    <%--Top  payable  This Month--%>
    <script type="text/javascript">
        function renderTopDebtorsChart(data) {
            if (typeof data === "string") {
                data = JSON.parse(data);
            }
            var labels = data.map(item => item.SUB_GL_NAME);
            var values = data.map(item => parseFloat(item.BALANCE));

            var ctx = document.getElementById('topDebtorsChart').getContext('2d');
            new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        data: values,
                        backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF'],
                        hoverBackgroundColor: ['#FF80A5', '#66B3FF', '#FFD666', '#66FFDC', '#BBAAFF']
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: { display: false, position: 'top' },
                        title: { display: true, text: 'Top 5 Payables' }
                    }
                }
            });
        }
    </script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            var days = [];
            var CashSales = [];
            var CashPurchases = [];;

            // Get all rows from the GridView table (excluding the header row)
            var tableRows = document.querySelectorAll("#GridView1 tr");

            for (var i = 1; i < tableRows.length; i++) {
                var cols = tableRows[i].getElementsByTagName("td");

                if (cols.length > 0) {
                    var day = cols[0].innerText.trim();
                    var CashSale = parseInt(cols[5].innerText.trim()) || 0;
                    var CashPurchase = parseInt(cols[3].innerText.trim()) || 0;

                    if (!isNaN(day) && day !== "") {
                        days.push(day);
                        CashSales.push(CashSale);
                        CashPurchases.push(CashPurchase);
                    }
                }
            }

            console.log("Days:", days);
            console.log("Sales:", CashSales);
            console.log("Purchases:", CashPurchases);

            if (days.length === 0) {
                console.warn("No data available for the chart.");
                return;
            }

            var ctx = document.getElementById("monthlySumaryChart").getContext("2d");

            new Chart(ctx, {
                type: "line",
                data: {
                    labels: days,
                    datasets: [
                        {
                            label: "Sales",
                            data: CashSales,
                            backgroundColor: "blue",
                            borderColor: "blue",
                            borderWidth: 2,
                            fill: false,
                            tension: 0.35
                        },

                        {
                            label: "Purchases",
                            data: CashPurchases,
                            backgroundColor: "red",
                            borderColor: "red",
                            borderWidth: 2,
                            fill: false,
                            tension: 0.3
                        }
                    ]
                },
                options: {
                    responsive: true,
                    plugins: {
                        tooltip: {
                            callbacks: {
                                label: function (tooltipItem) {
                                    let datasetLabel = tooltipItem.dataset.label.toUpperCase();
                                    let value = tooltipItem.raw;
                                    return `${datasetLabel}: ${value}`;
                                },
                                title: function (tooltipItems) {
                                    let index = tooltipItems[0].dataIndex;
                                    let date = days[index];
                                    return `Date: ${date}`;
                                }
                            }
                        }
                    },
                    scales: {
                        x: { title: { display: true, text: "Days of the Month" } },
                        y: { title: { display: true, text: "Amount" }, beginAtZero: true }
                    }
                }
            });
        });</script>

    <script type="text/javascript">
        function triggerPostBack() {
            __doPostBack('chkToggle', '');
        }
    </script>


    <div style="margin-top: -30px">


        <div class="row">
        </div>
        <div class="row">
            <div class="col-lg-1 col-sm-6">
                <div class="row" style="border-top: solid 5px; border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Purchase</span><br />
                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/images/icons/sales1.png" Width="40px" runat="server" OnClick="ImageButton1_Click" />
                    </div>
                </div>
                <div class="row" style="border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Sales</span>
                        <br />
                        <asp:ImageButton ID="ImageButton2" ImageUrl="~/images/icons/sales.png" Width="40px" runat="server" OnClick="ImageButton2_Click" />
                    </div>
                </div>
                <div class="row" style="border-top: solid 1px; border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Customer  </span>
                        <br />
                        <asp:ImageButton ID="ImageButton5" ImageUrl="~/images/icons/customer.png" Width="40px" runat="server" OnClick="ImageButton5_Click" />
                    </div>
                </div>
                <div class="row" style="border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Supplier </span>
                        <br />
                        <asp:ImageButton ID="ImageButton6" ImageUrl="~/images/icons/customer.png" Width="40px" runat="server" OnClick="ImageButton6_Click" />
                    </div>
                </div>
                <div class="row" style="border-top: solid 1px; border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Product  </span>
                        <br />
                        <asp:ImageButton ID="ImageButton7" ImageUrl="~/images/icons/product.png" Width="40px" runat="server" OnClick="ImageButton7_Click" />
                    </div>
                </div>

                <div class="row" style="border-top: solid 1px; border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Purchase </span>
                        <br />
                        <asp:ImageButton ID="ImageButton3" ImageUrl="~/images/icons/purchase.png" Width="40px" runat="server" OnClick="ImageButton3_Click" />
                    </div>
                </div>
                <div class="row" style="border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Sales </span>
                        <br />
                        <asp:ImageButton ID="ImageButton4" ImageUrl="~/images/icons/purchase.png" Width="40px" runat="server" OnClick="ImageButton4_Click" />
                    </div>
                </div>
                <div class="row" style="border-top: solid 1px; border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Receivables</span>
                        <br />
                        <asp:ImageButton ID="ImageButton8" ImageUrl="~/images/icons/purchase.png" Width="40px" runat="server" OnClick="ImageButton8_Click" />
                    </div>
                </div>
                <div class="row" style="border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Payables</span>
                        <br />
                        <asp:ImageButton ID="ImageButton9" ImageUrl="~/images/icons/purchase.png" Width="40px" runat="server" OnClick="ImageButton9_Click" />
                    </div>
                </div>
                <div class="row" style="border-left: solid 5px; border-color: #c9daf9; background-color: #00cccc;">
                    <div class="col-lg-12" style="text-align: center">
                        <span style="font-size: small">Ledger </span>
                        <br />
                        <asp:ImageButton ID="ImageButton10" ImageUrl="~/images/icons/purchase.png" Width="40px" runat="server" OnClick="ImageButton10_Click" />
                    </div>
                </div>
            </div>

            <div id="divView" runat="server" visible="false" class="toggle-container">
                View All:<br />
                <label class="switch">
                    <asp:CheckBox ID="ToggleSwitch" runat="server" AutoPostBack="true" OnCheckedChanged="ToggleSwitch_CheckedChanged" />
                    <span class="slider round"></span>
                </label>
            </div>
            <div class="col-lg-11">

                <!-- Chart Containers -->
                <div class="col-md-12" style="text-align: center;">
                    Monthly Transaction Summary
                </div>
                <div class="col-md-12">
                    <canvas id="monthlySumaryChart" width="400" height="100"></canvas>
                </div>
                <div class="row" style="border: solid 5px; border-color: #c9daf9; padding-bottom: 20px">
                    <div class="col-md-2">
                        <canvas id="chartDailySalesCanvas" width="500"></canvas>
                    </div>
                    <div class="col-md-2">
                        <canvas id="chartCashDailySalesCanvas" width="500"></canvas>
                    </div>
                    <div class="col-md-2">
                        <canvas id="chartDailyCreditSalesCanvas" width="500" height="300"></canvas>
                    </div>
                    <div class="col-md-2">
                        <canvas id="chartDailyPurchasesCanvas" width="500" height="300"></canvas>
                    </div>
                    <div class="col-md-2">
                        <canvas id="chartCashDailyPurchasesCanvas" width="500" height="300"></canvas>
                    </div>
                    <div class="col-md-2">
                        <canvas id="chartDailyCreditPurchasesCanvas" width="500" height="300"></canvas>
                    </div>


                </div>
                <!-- Canvas for the Chart -->
                <!-- Canvas for Operating Cost bar chart -->
                <div class="row" style="border: solid 5px; border-color: #c9daf9; padding-bottom: 20px">
                    <div class="col-md-3">
                        <canvas id="chartTopSalesIn30Canvas" width="500" height="500"></canvas>
                    </div>
                    <div class="col-md-3">
                        <canvas id="topCreditorsChart" width="400" height="400"></canvas>
                    </div>
                    <div class="col-md-3">
                        <canvas id="topDebtorsChart" width="400" height="400"></canvas>
                    </div>
                    <div class="col-md-3">
                        <canvas id="cashFlowChartCanvas" width="500" height="500"></canvas>
                    </div>
                </div>
                <div class="row" style="border: solid 5px; border-color: #c9daf9; padding-bottom: 20px">
                    <div class="col-md-3">
                        <canvas id="operatingCostLastSixChart" width="500" height="500"></canvas>
                    </div>

                </div>

            </div>

        </div>

    </div>



    <div>
        <asp:GridView Visible="false" ID="gridCBMS" runat="server" AutoGenerateColumns="False" OnRowDataBound="gridCBMS_RowDataBound" CssClass="gridtable">
            <Columns>
                <asp:TemplateField HeaderText="Sno">
                    <ItemTemplate>
                        <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice No.">
                    <ItemTemplate>
                        <asp:Label ID="lblPkID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("INVOICE_NUMBER") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice Date">
                    <ItemTemplate>
                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Bind("INVOICE_DATE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Name">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Bind("INVOICE_AMOUNT") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gridCBMSSalesReturn" Visible="false" runat="server" AutoGenerateColumns="False" OnRowDataBound="gridCBMSSalesReturn_RowDataBound" CssClass="gridtable">
            <Columns>
                <asp:TemplateField HeaderText="Sno">
                    <ItemTemplate>
                        <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sales Invoice ID">
                    <ItemTemplate>
                        <asp:Label ID="lblPkID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblSalesInvoiceID" runat="server" Text='<%# Bind("SALES_INVOICE_ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Credit Note Number">
                    <ItemTemplate>
                        <asp:Label ID="lblCreditNoteNumber" runat="server" Text='<%# Bind("CREDIT_NOTE_NUMBER") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Note Date">
                    <ItemTemplate>
                        <asp:Label ID="lblNoteDate" runat="server" Text='<%# Bind("NOTE_DATE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Name">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sales Return Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblSalesReturnAmount" runat="server" Text='<%# Bind("SALES_RETURN_AMOUNT") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

        <asp:GridView ID="GridView1" CssClass="hidden-grid" runat="server" AutoGenerateColumns="false" ClientIDMode="Static" ShowHeader="true" Width="80%" Visible="true" OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Day">
                    <ItemTemplate>
                        <asp:Label ID="lblDay" runat="server" Text='<%# Eval("Day") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Sales Date">
                    <ItemTemplate>
                        <asp:Label ID="lblSalesDate" runat="server" Text='<%# Eval("SalesDate") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Purchase Date">
                    <ItemTemplate>
                        <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Eval("PurchaseDate") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Cash Purchase">
                    <ItemTemplate>
                        <asp:Label ID="lblCashPurchase" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Credit Purchase">
                    <ItemTemplate>
                        <asp:Label ID="lblCreditPurchase" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Cash Sales">
                    <ItemTemplate>
                        <asp:Label ID="lblCashSales" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Credit Sales">
                    <ItemTemplate>
                        <asp:Label ID="lblCreditSales" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>


