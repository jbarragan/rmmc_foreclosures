using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using com.sp.rmmc.foreclosures.models;
using com.sp.rmmc.common.tools;

using Microsoft.VisualBasic.FileIO;
using com.sp.rmmc.common.models;

public partial class lossmitigation_Collections_Default : System.Web.UI.Page
{
    protected List<PreservationCode> all_preservation_codes = new List<PreservationCode>();
    protected List<TypeList> preservation_loan_types = new List<TypeList>();


    protected class ServiceList : List<PreservationCode>{}
    protected class TypeList : List<ServiceList>{}

    protected TypeList currentTypeList = new TypeList();
    protected ServiceList currentServiceList = new ServiceList();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Submit1.ServerClick += new System.EventHandler(this.Submit1_ServerClick);
        all_preservation_codes = PreservationCode.get_all_preservation_codes_by_order("order by code");
    }

    protected void btnLoanDisbursements_Click(object sender, EventArgs e)
    {
        Response.Redirect("LoanDisbursements.aspx?loan_id=" + this.txtLoanId.Text);
    }
    
    protected void lbExport_Click(object sender, EventArgs e)
    {
        string filename = "PreservationCodes";
        CSVExport.writeHeadersAndCSV(filename, "Code,Type,Description,Service,Allowance,Effective Date,Enabled", this.all_preservation_codes);
    }

    private void Submit1_ServerClick(object sender, System.EventArgs e)
    {
        if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
        {
            string fn = System.IO.Path.GetFileName(File1.PostedFile.FileName);
            string SaveLocation = Server.MapPath("Data") + "\\" + fn;
            try
            {
                File1.PostedFile.SaveAs(SaveLocation);
                //lbl_success.Text = "The file has been uploaded.";
                processUploadedFile(SaveLocation);
            }

            catch (Exception ex)
            {
                lbl_error.Text = "Error: " + ex.Message;
            }
        }
        else
        {
            lbl_error.Text = "Please select a file to upload.";
        }
    }

    private void processUploadedFile(string file_location)
    {
        TextFieldParser parser = new TextFieldParser(file_location);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;
        List<PreservationCode> preservation_codes = new List<PreservationCode>();
        while (!parser.EndOfData)
        {
            //Processing row
            string[] fields = parser.ReadFields();
            preservation_codes.Add( this.parse(fields) );
        }
        parser.Close();
        string error = "";
        for (int i = 0; i < preservation_codes.Count; i++)
        {
            PreservationCode pc = preservation_codes[i];
            if (pc.validation_error != "")
            {
                if (i == 0 && pc.code == "Code") continue;
                error += "Line " + (i + 1).ToString() + ": " + pc.validation_error + "<br/>";
            }
        }
        this.lbl_error.Text = error;
        if (error.Length > 0) return;
        int updates = 0;
        int inserts = 0;
        foreach (PreservationCode pc in preservation_codes)
        {
            if (pc.code.ToUpper() == "CODE") continue;
            PreservationCode pcdb = PreservationCode.get(pc.code);
            if (pcdb.code == pc.code) { pc.id = pcdb.id; PreservationCode.update(pc.code, pc); pc.getPCA().insert(); updates++; }
            else { PreservationCode.create(pc); pc.getPCA().insert(); inserts++; }
        }
        this.lbl_success.Text = "File processing successful. Updated " + updates.ToString() + " and Inserted " + inserts.ToString() + " codes";
    }

    private PreservationCode parse(string[] fields)
    {
        PreservationCode pc = new PreservationCode();
        if( fields.Length < 7 ){
            pc.validation_error = "Invalid file format, file must follow this format: Code,Type,Description,Service,Allowance,Effective Date,Enabled";
            return pc;
        }
        if( fields[0] == null || fields[0].Length == 0){
            pc.validation_error = "Code is required";
            return pc;
        }
        pc.code = fields[0];
        if (fields[1] == null || fields[1].Length == 0 ||
            !(fields[1] == "FHA" || fields[1] == "CONV" || fields[1] == "VA"))
        {
            pc.validation_error = "Type requires the following values: FHA, CONV, and VA";
            return pc;
        }
        pc.type = fields[1];
        pc.description = fields[2];
        pc.general_service = fields[3];
        
        decimal allowance = convertStringToDecimal(fields[4]);
        if (allowance < 0M)
        {
            pc.validation_error = "Allowance requires to be a positive number";
            return pc;
        }
        pc.allowance = allowance;

        DateTimeObject date = convertStringToDate(fields[5]);
        if (date.isNull)
        {
            pc.validation_error = "Effective Date requires to be a valid date with MM/DD/YYYY format";
        }
        pc.effective_date = date;

        pc.enabled = (fields[6] == null || fields[6].ToUpper() != "FALSE");
        return pc;
    }

    protected decimal convertStringToDecimal(string s)
    {
        decimal d = -1.0M;
        try
        {
            string ds = Regex.Replace(s, "[^0-9.-]", "");
            d = Decimal.Parse(ds);
        }
        catch (Exception)
        {
        }
        return d;
    }


    protected DateTimeObject convertStringToDate(string s)
    {
        DateTimeObject d = new DateTimeObject();
        try
        {
            d.set_date(DateTime.Parse(s));
            return d;
        }
        catch (Exception)
        {
        }
        return d;
    }
}