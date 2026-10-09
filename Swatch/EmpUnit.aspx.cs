using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Swatch_EmpUnit : System.Web.UI.Page
{
    PR_UNIT UEnt = new PR_UNIT();
    PR_UNITService USer = new PR_UNITService();

    PR_DIVISION DEnt = new PR_DIVISION();
    PR_DIVISIONService DSer = new PR_DIVISIONService();

    PR_DIVISION_EMPLOYEE MEnt = new PR_DIVISION_EMPLOYEE();
    PR_DIVISION_EMPLOYEEService MSer = new PR_DIVISION_EMPLOYEEService();

    EMPLOYEES EmpEnt = new EMPLOYEES();
    EMPLOYEESService EmpSer = new EMPLOYEESService();

    EntityList theList = new EntityList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            SetTab(0);
    }



    protected void btnTab_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        SetTab(Convert.ToInt32(btn.CommandArgument));
    }

    private void SetTab(int index)
    {
        mvEmp.ActiveViewIndex = index;

        btnTabUnit.CssClass = "btn-view-primary" + (index == 0 ? " active" : "");
        btnTabDivision.CssClass = "btn-view-primary" + (index == 1 ? " active" : "");
        btnTabMap.CssClass = "btn-view-primary" + (index == 2 ? " active" : "");

        if (index == 0)
        {
            gridUnit.EditIndex = -1;
            gridUnit.PageIndex = 0;
            LoadUnitGrid();
        }
        else if (index == 1)
        {
            gridDivision.EditIndex = -1;
            gridDivision.PageIndex = 0;
            LoadDivisionGrid();
        }
        else
        {
            gridMap.PageIndex = 0;
            LoadMapGrid();
        }
    }



    private void LoadUnitGrid()
    {
        UEnt = new PR_UNIT();
        theList = USer.GetAll(UEnt);

        if (theList.Count == 0)
            theList.Add(UEnt);

        gridUnit.DataSource = theList;
        gridUnit.DataBind();
    }

    protected void gridUnit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridUnit.EditIndex = -1;
        gridUnit.PageIndex = e.NewPageIndex;
        LoadUnitGrid();
    }

    protected void gridUnit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlHeadOfUnitH = (DropDownList)e.Row.FindControl("ddlHeadOfUnitH");

            if (ddlHeadOfUnitH != null)
                LoadEmployeeDropDown(ddlHeadOfUnitH);
        }


        Label lblPK_ID = (Label)e.Row.FindControl("lblPK_ID");
        bool isPlaceholder = (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text));

        if ((e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            SelectStatus(e.Row);

            DropDownList ddlHeadOfUnit = (DropDownList)e.Row.FindControl("ddlHeadOfUnit");
            Label lblHeadOfUnitIDE = (Label)e.Row.FindControl("lblHeadOfUnitIDE");

            if (ddlHeadOfUnit != null)
            {
                LoadEmployeeDropDown(ddlHeadOfUnit);

                if (lblHeadOfUnitIDE != null && ddlHeadOfUnit.Items.FindByValue(lblHeadOfUnitIDE.Text) != null)
                    ddlHeadOfUnit.SelectedValue = lblHeadOfUnitIDE.Text;
            }
        }
        else
        {
            ShowStatusPill(e.Row);
            HideButtonsIfPlaceholder(e.Row, isPlaceholder);

            Label lblHeadOfUnitID = (Label)e.Row.FindControl("lblHeadOfUnitID");
            Label lblHeadOfUnit = (Label)e.Row.FindControl("lblHeadOfUnit");

            if (lblHeadOfUnit != null && lblHeadOfUnitID != null)
                lblHeadOfUnit.Text = GetEmployeeName(lblHeadOfUnitID.Text);
        }
    }

    protected void gridUnit_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridUnit.EditIndex = e.NewEditIndex;
        LoadUnitGrid();
    }

    protected void gridUnit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridUnit.EditIndex = -1;
        LoadUnitGrid();
    }

    protected void gridUnit_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridUnit.Rows[e.RowIndex];

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");
        TextBox txtUnitName = (TextBox)row.FindControl("txtUnitNameE");
        DropDownList ddlHeadOfUnit = (DropDownList)row.FindControl("ddlHeadOfUnit");
        TextBox txtDescription = (TextBox)row.FindControl("txtDescriptionE");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        if (string.IsNullOrWhiteSpace(txtUnitName.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Unit Name can not be empty.");
            return;
        }

        UEnt = new PR_UNIT();
        UEnt.PK_ID = lblPK_ID.Text;
        UEnt = (PR_UNIT)USer.GetSingle(UEnt);

        if (UEnt != null)
        {
            UEnt.UNIT_NAME = txtUnitName.Text.Trim();
            UEnt.HEAD_OF_UNIT = ddlHeadOfUnit.SelectedValue;
            UEnt.DESCRIPTION = txtDescription.Text.Trim();
            UEnt.STATUS = ddlStatus.SelectedValue;

            USer.Update(UEnt);

            HelperFunction.MsgBox(this, this.GetType(), "Updated");
        }

        gridUnit.EditIndex = -1;
        LoadUnitGrid();
    }

    protected void gridUnit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = gridUnit.Rows[e.RowIndex];
        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        UEnt = new PR_UNIT();
        UEnt.PK_ID = lblPK_ID.Text;
        UEnt = (PR_UNIT)USer.GetSingle(UEnt);

        if (UEnt != null)
        {
            USer.Delete(UEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Deleted");
        }

        gridUnit.EditIndex = -1;
        LoadUnitGrid();
    }

    protected void btnAddUnit_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridUnit.HeaderRow;

        TextBox txtUnitName = (TextBox)row.FindControl("txtUnitName");
        DropDownList ddlHeadOfUnit = (DropDownList)row.FindControl("ddlHeadOfUnitH");
        TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatusH");

        if (string.IsNullOrWhiteSpace(txtUnitName.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Unit Name can not be empty.");
            return;
        }

        UEnt = new PR_UNIT();
        UEnt.UNIT_NAME = txtUnitName.Text.Trim();
        UEnt.HEAD_OF_UNIT = ddlHeadOfUnit.SelectedValue;
        UEnt.DESCRIPTION = txtDescription.Text.Trim();
        UEnt.STATUS = ddlStatus.SelectedValue;

        USer.Insert(UEnt);

        HelperFunction.MsgBox(this, this.GetType(), "Inserted");

        LoadUnitGrid();
    }


    private void LoadDivisionGrid()
    {
        DEnt = new PR_DIVISION();
        theList = DSer.GetAll(DEnt);

        if (theList.Count == 0)
            theList.Add(DEnt);

        gridDivision.DataSource = theList;
        gridDivision.DataBind();
    }

    protected void gridDivision_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridDivision.EditIndex = -1;
        gridDivision.PageIndex = e.NewPageIndex;
        LoadDivisionGrid();
    }

    protected void gridDivision_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlUnitH = (DropDownList)e.Row.FindControl("ddlUnitH");
            if (ddlUnitH != null)
                LoadUnitDropDown(ddlUnitH);
        }

        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        Label lblPK_ID = (Label)e.Row.FindControl("lblPK_ID");
        bool isPlaceholder = (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text));

        if ((e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            SelectStatus(e.Row);

            DropDownList ddlUnit = (DropDownList)e.Row.FindControl("ddlUnit");
            Label lblUnitIDE = (Label)e.Row.FindControl("lblUnitIDE");

            if (ddlUnit != null)
            {
                LoadUnitDropDown(ddlUnit);

                if (lblUnitIDE != null && ddlUnit.Items.FindByValue(lblUnitIDE.Text) != null)
                    ddlUnit.SelectedValue = lblUnitIDE.Text;
            }
        }
        else
        {
            ShowStatusPill(e.Row);
            HideButtonsIfPlaceholder(e.Row, isPlaceholder);

            Label lblUnitID = (Label)e.Row.FindControl("lblUnitID");
            Label lblUnit = (Label)e.Row.FindControl("lblUnit");

            if (lblUnit != null && lblUnitID != null)
                lblUnit.Text = GetUnitName(lblUnitID.Text);
        }
    }

    protected void gridDivision_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridDivision.EditIndex = e.NewEditIndex;
        LoadDivisionGrid();
    }

    protected void gridDivision_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridDivision.EditIndex = -1;
        LoadDivisionGrid();
    }

    protected void gridDivision_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridDivision.Rows[e.RowIndex];

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");
        DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
        TextBox txtDivisionName = (TextBox)row.FindControl("txtDivisionNameE");
        TextBox txtDescription = (TextBox)row.FindControl("txtDescriptionE");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");

        if (string.IsNullOrEmpty(ddlUnit.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Unit can not be empty.");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtDivisionName.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Division Name can not be empty.");
            return;
        }

        DEnt = new PR_DIVISION();
        DEnt.PK_ID = lblPK_ID.Text;
        DEnt = (PR_DIVISION)DSer.GetSingle(DEnt);

        if (DEnt != null)
        {
            DEnt.UNIT_ID = ddlUnit.SelectedValue;
            DEnt.DIVISION_NAME = txtDivisionName.Text.Trim();
            DEnt.DESCRIPTION = txtDescription.Text.Trim();
            DEnt.STATUS = ddlStatus.SelectedValue;

            DSer.Update(DEnt);

            HelperFunction.MsgBox(this, this.GetType(), "Updated");
        }

        gridDivision.EditIndex = -1;
        LoadDivisionGrid();
    }

    protected void gridDivision_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = gridDivision.Rows[e.RowIndex];
        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        DEnt = new PR_DIVISION();
        DEnt.PK_ID = lblPK_ID.Text;
        DEnt = (PR_DIVISION)DSer.GetSingle(DEnt);

        if (DEnt != null)
        {
            DSer.Delete(DEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Deleted");
        }

        gridDivision.EditIndex = -1;
        LoadDivisionGrid();
    }

    protected void btnAddDivision_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridDivision.HeaderRow;

        DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnitH");
        TextBox txtDivisionName = (TextBox)row.FindControl("txtDivisionName");
        TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatusH");

        if (string.IsNullOrEmpty(ddlUnit.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Unit can not be empty.");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtDivisionName.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Division Name can not be empty.");
            return;
        }

        DEnt = new PR_DIVISION();
        DEnt.UNIT_ID = ddlUnit.SelectedValue;
        DEnt.DIVISION_NAME = txtDivisionName.Text.Trim();
        DEnt.DESCRIPTION = txtDescription.Text.Trim();
        DEnt.STATUS = ddlStatus.SelectedValue;

        DSer.Insert(DEnt);

        HelperFunction.MsgBox(this, this.GetType(), "Inserted");

        LoadDivisionGrid();
    }


    private void LoadMapGrid()
    {
        MEnt = new PR_DIVISION_EMPLOYEE();
        theList = MSer.GetAll(MEnt);

        if (theList.Count == 0)
            theList.Add(MEnt);

        gridMap.DataSource = theList;
        gridMap.DataBind();
    }

    protected void gridMap_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridMap.PageIndex = e.NewPageIndex;
        LoadMapGrid();
    }

    protected void gridMap_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlMapDivisionH = (DropDownList)e.Row.FindControl("ddlMapDivisionH");
            DropDownList ddlMapEmployeeH = (DropDownList)e.Row.FindControl("ddlMapEmployeeH");

            if (ddlMapDivisionH != null)
                LoadDivisionDropDown(ddlMapDivisionH);

            if (ddlMapEmployeeH != null)
                LoadEmployeeDropDown(ddlMapEmployeeH);
        }

        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        Label lblPK_ID = (Label)e.Row.FindControl("lblPK_ID");
        bool isPlaceholder = (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text));

        HideButtonsIfPlaceholder(e.Row, isPlaceholder);

        Label lblDivisionID = (Label)e.Row.FindControl("lblDivisionID");
        Label lblDivision = (Label)e.Row.FindControl("lblDivision");
        Label lblEmployeeID = (Label)e.Row.FindControl("lblEmployeeID");
        Label lblEmployee = (Label)e.Row.FindControl("lblEmployee");

        if (lblDivision != null && lblDivisionID != null)
            lblDivision.Text = GetDivisionName(lblDivisionID.Text);

        if (lblEmployee != null && lblEmployeeID != null)
            lblEmployee.Text = GetEmployeeName(lblEmployeeID.Text);
    }

    protected void btnMap_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridMap.HeaderRow;

        DropDownList ddlDivision = (DropDownList)row.FindControl("ddlMapDivisionH");
        DropDownList ddlEmployee = (DropDownList)row.FindControl("ddlMapEmployeeH");

        if (string.IsNullOrEmpty(ddlDivision.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Division can not be empty.");
            return;
        }
        if (string.IsNullOrEmpty(ddlEmployee.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Employee can not be empty.");
            return;
        }

        MEnt = new PR_DIVISION_EMPLOYEE();
        MEnt.DIVISION_ID = ddlDivision.SelectedValue;
        MEnt.EMPLOYEE_ID = ddlEmployee.SelectedValue;
        EntityList existing = MSer.GetAll(MEnt);

        if (existing.Count > 0)
        {
            HelperFunction.MsgBox(this, this.GetType(), "This division and employee are already mapped.");
            return;
        }

        MEnt = new PR_DIVISION_EMPLOYEE();
        MEnt.DIVISION_ID = ddlDivision.SelectedValue;
        MEnt.EMPLOYEE_ID = ddlEmployee.SelectedValue;

        MSer.Insert(MEnt);

        HelperFunction.MsgBox(this, this.GetType(), "Mapped");

        LoadMapGrid();
    }

    protected void gridMap_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = gridMap.Rows[e.RowIndex];
        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        MEnt = new PR_DIVISION_EMPLOYEE();
        MEnt.PK_ID = lblPK_ID.Text;
        MEnt = (PR_DIVISION_EMPLOYEE)MSer.GetSingle(MEnt);

        if (MEnt != null)
        {
            MSer.Delete(MEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Mapping removed");
        }

        LoadMapGrid();
    }


    private void LoadUnitDropDown(DropDownList ddl)
    {
        UEnt = new PR_UNIT();
        UEnt.STATUS = "1";

        ddl.DataSource = USer.GetAll(UEnt);
        ddl.DataTextField = "UNIT_NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Unit --", ""));
    }

    private void LoadDivisionDropDown(DropDownList ddl)
    {
        DEnt = new PR_DIVISION();
        DEnt.STATUS = "1";

        ddl.DataSource = DSer.GetAll(DEnt);
        ddl.DataTextField = "DIVISION_NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Division --", ""));
    }

    private void LoadEmployeeDropDown(DropDownList ddl)
    {
        EmpEnt = new EMPLOYEES();

        ddl.DataSource = EmpSer.GetAll(EmpEnt);
        ddl.DataTextField = "FIRSTNAME";
        ddl.DataValueField = "EMPLOYEEID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Employee --", ""));
    }

    private string GetUnitName(string unitId)
    {

        PR_UNIT ent = new PR_UNIT();
        ent.PK_ID = unitId;
        ent = (PR_UNIT)USer.GetSingle(ent);

        return ent != null ? ent.UNIT_NAME : "";
    }

    private string GetDivisionName(string divisionId)
    {
        if (string.IsNullOrEmpty(divisionId))
            return "";

        PR_DIVISION ent = new PR_DIVISION();
        ent.PK_ID = divisionId;
        ent = (PR_DIVISION)DSer.GetSingle(ent);

        return ent != null ? ent.DIVISION_NAME : "";
    }

    private string GetEmployeeName(string employeeId)
    {

        EMPLOYEES ent = new EMPLOYEES();
        ent.EMPLOYEEID = employeeId;
        ent = (EMPLOYEES)EmpSer.GetSingle(ent);

        return ent.FIRSTNAME;
    }

    private void ShowStatusPill(GridViewRow row)
    {
        Label lblStatus = (Label)row.FindControl("lblStatus");
        Label lblStatusShow = (Label)row.FindControl("lblStatusShow");

        if (lblStatus == null || lblStatusShow == null || string.IsNullOrEmpty(lblStatus.Text))
            return;

        if (lblStatus.Text == "1")
        {
            lblStatusShow.Text = "Active";
            lblStatusShow.CssClass = "status-pill status-active";
        }
        else
        {
            lblStatusShow.Text = "Inactive";
            lblStatusShow.CssClass = "status-pill status-inactive";
        }
    }

    private void SelectStatus(GridViewRow row)
    {
        Label lblStatusE = (Label)row.FindControl("lblStatusE");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");

        if (lblStatusE != null && ddlStatus != null && ddlStatus.Items.FindByValue(lblStatusE.Text) != null)
            ddlStatus.SelectedValue = lblStatusE.Text;
    }

    private void HideButtonsIfPlaceholder(GridViewRow row, bool isPlaceholder)
    {
        if (!isPlaceholder)
            return;

        ImageButton btnEdit = (ImageButton)row.FindControl("btnEdit");
        ImageButton btnDelete = (ImageButton)row.FindControl("btnDelete");

        if (btnEdit != null) btnEdit.Visible = false;
        if (btnDelete != null) btnDelete.Visible = false;
    }
}