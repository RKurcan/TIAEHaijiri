using Riddhasoft.EHajiriPro.Desktop.Common;
using Riddhasoft.EHajiriPro.Desktop.ViewModel;
using Riddhasoft.Employee.Entities;
using Riddhasoft.Employee.Services;
using Riddhasoft.Globals.Conversion;
using Riddhasoft.OfficeSetup.Services;
using Riddhasoft.Services.Common;
using Riddhasoft.Services.User;
using Riddhasoft.User.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Office_Management
{
    public partial class EmployeeFrm : Form
    {
        EEmployee _employeeData = null;
        private string imageExt = "";
        private int branchId = RiddhaSession.BranchId;
        public EmployeeFrm()
        {
            InitializeComponent();
            _employeeData = new EEmployee();
            populateDesignation();
            populateDepartment();
            populateGradeGroup();
            populateShift();
            btnSave.Text = "Create";
            cmbReligion.SelectedIndex = 0;
            cmbBloodGroup.SelectedIndex = 0;
            cmbMaritalStatus.SelectedIndex = 0;
            cmbShiftType.SelectedIndex = 0;
            cmbGender.SelectedIndex = 0;
            cmbDesignation.SelectedIndex = 0;
            cmbShift.SelectedIndex = 0;
            cmbGradeGroup.SelectedIndex = 0;
            populateEmployeeGrid();
        }

        #region ComboBoxBindings
        public void populateDesignation()
        {
            var designationServices = new SDesignation();
            var result = designationServices.List().Data.Where(x => x.BranchId == RiddhaSession.BranchId).ToList();
            cmbDesignation.DisplayMember = "Name";
            cmbDesignation.ValueMember = "Id";
            cmbDesignation.DataSource = result;
        }
        public void populateDepartment()
        {
            var departmentServices = new SDepartment();
            var result = departmentServices.List().Data.ToList();
            cmbDepartment.DisplayMember = "Name";
            cmbDepartment.ValueMember = "Id";
            cmbDepartment.DataSource = result;
        }

        public void populateSectionByDepartment()
        {
            var sectionServices = new SSection();
            int departmentId = cmbDepartment.SelectedValue == null ? 0 : (cmbDepartment.SelectedValue.ToInt());
            var result = sectionServices.List().Data.Where(x => x.DepartmentId == departmentId).ToList();
            cmbSection.DisplayMember = "Name";
            cmbSection.ValueMember = "Id";
            cmbSection.DataSource = result;
        }

        public void populateGradeGroup()
        {
            var gradeGroupServices = new SGradeGroup();
            var result = gradeGroupServices.List().Data.ToList();
            cmbGradeGroup.DisplayMember = "Name";
            cmbGradeGroup.ValueMember = "Id";
            cmbGradeGroup.DataSource = result;
        }

        public void populateShift()
        {
            var shiftServices = new SShift();
            var result = shiftServices.List().Data.ToList();
            cmbShift.DisplayMember = "ShiftName";
            cmbShift.ValueMember = "Id";
            cmbShift.DataSource = result;
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateSectionByDepartment();
        }

        #endregion

        private void populateEmployeeGrid()
        {
            SEmployee employeeServices = new SEmployee();
            SUser userServices = new SUser();
            var user = userServices.List().Data.ToList();
            var data = (from c in employeeServices.List().Data.ToList()
                        select new EmployeeGridVm()
                        {
                            Id = c.Id,
                            Department = c.Section == null ? "" : c.Section.Department.Name,
                            Section = c.Section == null ? "" : c.Section.Name,
                            Designation = c.Designation == null ? "" : c.Designation.Name,
                            EmployeeCode = c.Code,
                            EmployeeName = c.Name,
                            ShiftType = c.ShiftTypeId == 0 ? "Fixed" : c.ShiftTypeId == 1 ? "Weekly" : "Monthly",
                            UserId = c.UserId
                        }).ToList();
            data.ForEach(x => x.LoginStatus = getLoginStatus(user, x.UserId) == true ? "Off" : "On");
            employeeGridView.DataSource = data;

        }
        private bool getLoginStatus(List<EUser> userQuery, int? userId)
        {
            if (userId == null)
            {
                return true;
            }
            else
            {
                var user = userQuery.Where(x => x.Id == userId).Single();
                return user.IsSuspended;
            }
        }

        public void processResult<T>(Services.Common.ServiceResult<T> result)
        {
            if (result.Status == ResultStatus.Ok)
            {
                //MessageBox.Show(result.Message);
                populateEmployeeGrid();
                createToolStripMenuItem_Click(null, null);
                Reset();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (txtEmployeeCode.Text == "")
            {
                MessageBox.Show("Employee Code is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeeCode.Focus();
                return;
            }
            if (txtEmpName.Text == "")
            {
                MessageBox.Show("Employee Name is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmpName.Focus();
                return;
            }
            if (txtEmpDeviceCode.Text == "")
            {
                MessageBox.Show("Employee Device Code is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmpDeviceCode.Focus();
                return;
            }
            if (txtEmpDeviceCode.Text != null)
            {
                try
                {
                    int.Parse(txtEmpDeviceCode.Text);
                }
                catch (Exception)
                {

                    MessageBox.Show("Invalid Data Type. Please add only numeric value", "Warning",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmpDeviceCode.Focus();
                    return;
                }

            }
            if (cmbDepartment.SelectedValue == null)
            {
                MessageBox.Show("Department is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDepartment.Focus();
                return;
            }
            if (cmbSection.SelectedValue == null)
            {
                MessageBox.Show("Section is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSection.Focus();
                return;
            }
            if (cmbDesignation.SelectedValue == null)
            {
                MessageBox.Show("Designation is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDesignation.Focus();
                return;
            }
            if (cmbShift.SelectedValue == null)
            {
                MessageBox.Show("Shift is required.", "Warning",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbShift.Focus();
                return;
            }

            if (_employeeData.Id == 0)
            {
                SEmployee employeeServices = new SEmployee();
                PrepareData();

                List<int> SelectedWeeklyOff = new List<int>();
                if (chkWeeklyOffSunday.Checked)
                {
                    SelectedWeeklyOff.Add(0);
                }
                if (chkWeeklyOffMonday.Checked)
                {
                    SelectedWeeklyOff.Add(1);
                }
                if (chkWeeklyOffTuesday.Checked)
                {
                    SelectedWeeklyOff.Add(2);
                }
                if (chkWeeklyOffWednesday.Checked)
                {
                    SelectedWeeklyOff.Add(3);
                }
                if (chkWeeklyOffThursday.Checked)
                {
                    SelectedWeeklyOff.Add(4);
                }
                if (chkWeeklyOffFriday.Checked)
                {
                    SelectedWeeklyOff.Add(5);
                }
                if (chkWeeklyOffSaturday.Checked)
                {
                    SelectedWeeklyOff.Add(6);
                }
                int[] weeklyOffArrayToSave = SelectedWeeklyOff.ToArray();
                if (_employeeData.DateOfBirth.HasValue)
                {
                    DateTime value;
                    bool success = DateTime.TryParse(_employeeData.DateOfBirth.GetValueOrDefault().ToString(), out value);
                    if (success)
                    {
                        if (default(DateTime) == value)
                        {
                            _employeeData.DateOfBirth = null;
                        }
                        else
                        {
                            _employeeData.DateOfBirth = value;
                        }
                    }
                    else
                    {
                        _employeeData.DateOfBirth = null;
                    }
                    //string dob = _employeeData.DateOfBirth.HasValue ? _employeeData.DateOfBirth.Value.ToString() : string.Empty;
                    //if (dob == "0001/01/01 12:00:00 AM" || string.IsNullOrEmpty(dob))
                    //{
                    //    _employeeData.DateOfBirth = null;
                    //}
                }
                if (_employeeData.DateOfJoin.HasValue)
                {
                    DateTime value;
                    bool success = DateTime.TryParse(_employeeData.DateOfJoin.GetValueOrDefault().ToString(), out value);
                    if (success)
                    {
                        if (default(DateTime) == value)
                        {
                            _employeeData.DateOfJoin = null;
                        }
                        else
                        {
                            _employeeData.DateOfJoin = value;
                        }
                    }
                    else
                    {
                        _employeeData.DateOfJoin = null;
                    }

                }
                if (_employeeData.IssueDate.HasValue)
                {
                    DateTime value;
                    bool success = DateTime.TryParse(_employeeData.IssueDate.GetValueOrDefault().ToString(), out value);
                    if (success)
                    {
                        if (default(DateTime) == value)
                        {
                            _employeeData.IssueDate = null;
                        }
                        else
                        {
                            _employeeData.IssueDate = value;
                        }
                    }
                    else
                    {
                        _employeeData.IssueDate = null;
                    }
                }
                var result = employeeServices.Add(_employeeData, cmbShift.SelectedValue.ToInt(), weeklyOffArrayToSave);
                if (result.Status == ResultStatus.Ok)
                {
                    btnSave.Text = "Create";
                    Reset();
                    populateEmployeeGrid();
                }
                MessageBox.Show("Added Sucessfully", "Sucess",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                List<int> SelectedWeeklyOff = new List<int>();
                if (chkWeeklyOffSunday.Checked)
                {
                    SelectedWeeklyOff.Add(0);
                }
                if (chkWeeklyOffMonday.Checked)
                {
                    SelectedWeeklyOff.Add(1);
                }
                if (chkWeeklyOffTuesday.Checked)
                {
                    SelectedWeeklyOff.Add(2);
                }
                if (chkWeeklyOffWednesday.Checked)
                {
                    SelectedWeeklyOff.Add(3);
                }
                if (chkWeeklyOffThursday.Checked)
                {
                    SelectedWeeklyOff.Add(4);
                }
                if (chkWeeklyOffFriday.Checked)
                {
                    SelectedWeeklyOff.Add(5);
                }
                if (chkWeeklyOffSaturday.Checked)
                {
                    SelectedWeeklyOff.Add(6);
                }
                int[] weeklyOffArrayToSave = SelectedWeeklyOff.ToArray();
                PrepareData();
                var obj = _employeeData;
                SEmployee employeeServices = new SEmployee();
                ParseVmToModel(obj);
                if (_employeeData.DateOfBirth.HasValue)
                {
                    DateTime value;
                    bool success = DateTime.TryParse(_employeeData.DateOfBirth.GetValueOrDefault().ToString(), out value);
                    if (success)
                    {
                        if (default(DateTime) == value)
                        {
                            _employeeData.DateOfBirth = null;
                        }
                        else
                        {
                            _employeeData.DateOfBirth = value;
                        }
                    }
                    else
                    {
                        _employeeData.DateOfBirth = null;
                    }
                    //string dob = _employeeData.DateOfBirth.HasValue ? _employeeData.DateOfBirth.Value.ToString() : string.Empty;
                    //if (dob == "0001/01/01 12:00:00 AM" || string.IsNullOrEmpty(dob))
                    //{
                    //    _employeeData.DateOfBirth = null;
                    //}
                }
                if (_employeeData.DateOfJoin.HasValue)
                {
                    DateTime value;
                    bool success = DateTime.TryParse(_employeeData.DateOfJoin.GetValueOrDefault().ToString(), out value);
                    if (success)
                    {
                        if (default(DateTime) == value)
                        {
                            _employeeData.DateOfJoin = null;
                        }
                        else
                        {
                            _employeeData.DateOfJoin = value;
                        }
                    }
                    else
                    {
                        _employeeData.DateOfJoin = null;
                    }

                }
                if (_employeeData.IssueDate.HasValue)
                {
                    DateTime value;
                    bool success = DateTime.TryParse(_employeeData.IssueDate.GetValueOrDefault().ToString(), out value);
                    if (success)
                    {
                        if (default(DateTime) == value)
                        {
                            _employeeData.IssueDate = null;
                        }
                        else
                        {
                            _employeeData.IssueDate = value;
                        }
                    }
                    else
                    {
                        _employeeData.IssueDate = null;
                    }
                }
                var result = employeeServices.Update(_employeeData, cmbShift.SelectedValue.ToInt(), weeklyOffArrayToSave);
                if (result.Status == ResultStatus.Ok)
                {
                    btnSave.Text = "Create";
                    Reset();
                    populateEmployeeGrid();
                }
                MessageBox.Show("Updated Sucessfully", "Sucess",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Reset()
        {
            _employeeData = new EEmployee();
            setInputValue();
            cmbDesignation.SelectedIndex = 0;
            cmbShift.SelectedIndex = 0;
            cmbGradeGroup.SelectedIndex = 0;
        }

        private void ParseVmToModel(EEmployee obj)
        {
            SEmployee employeeServices = new SEmployee();
            var emp = employeeServices.List().Data.Where(x => x.Id == obj.Id).FirstOrDefault();
            _employeeData = new EEmployee();
            _employeeData.Id = emp.Id;
            _employeeData.AllowedEarlyOut = obj.AllowedEarlyOut;
            _employeeData.AllowedLateIn = obj.AllowedLateIn;
            _employeeData.BloodGroup = obj.BloodGroup;
            _employeeData.BranchId = obj.BranchId;
            _employeeData.CitizenNo = obj.CitizenNo;
            _employeeData.Code = obj.Code;
            _employeeData.ConsiderTimeLoss = obj.ConsiderTimeLoss;
            _employeeData.DateOfBirth = obj.DateOfBirth;
            _employeeData.DateOfJoin = obj.DateOfJoin;
            _employeeData.DesignationId = obj.DesignationId;
            _employeeData.DeviceCode = obj.DeviceCode;
            _employeeData.Email = obj.Email;
            _employeeData.EmploymentStatus = obj.EmploymentStatus;
            _employeeData.FourPunch = obj.FourPunch;
            _employeeData.Gender = obj.Gender;
            _employeeData.GradeGroupId = obj.GradeGroupId;
            _employeeData.HalfDayMarking = obj.HalfDayMarking;
            _employeeData.HalfdayWorkingHour = obj.HalfdayWorkingHour;
            _employeeData.ImageUrl = obj.ImageUrl;
            _employeeData.IsManager = obj.IsManager;
            _employeeData.IsOTAllowed = obj.IsOTAllowed;
            _employeeData.IssueDate = obj.IssueDate;
            _employeeData.IssueDistict = obj.IssueDistict;
            _employeeData.MaritialStatus = obj.MaritialStatus;
            _employeeData.MaxOTHour = obj.MaxOTHour;
            _employeeData.MaxWorkingHour = obj.MaxWorkingHour;
            _employeeData.MinOTHour = obj.MinOTHour;
            _employeeData.Mobile = obj.Mobile;
            _employeeData.MultiplePunch = obj.MultiplePunch;
            _employeeData.Name = obj.Name;
            _employeeData.NameNp = obj.NameNp;
            _employeeData.NoPunch = obj.NoPunch;
            _employeeData.PassportNo = obj.PassportNo;
            _employeeData.PermanentAddress = obj.PermanentAddress;
            _employeeData.PermanentAddressNp = obj.PermanentAddressNp;
            _employeeData.PresentMarkingDuration = obj.PresentMarkingDuration;
            _employeeData.Religion = obj.Religion;
            _employeeData.ReportingManagerId = emp.ReportingManagerId;
            _employeeData.SectionId = obj.SectionId;
            _employeeData.ShiftTypeId = obj.ShiftTypeId;
            _employeeData.ShortDayWorkingHour = obj.ShortDayWorkingHour;
            _employeeData.SinglePunch = obj.SinglePunch;
            _employeeData.TemporaryAddress = obj.TemporaryAddress;
            _employeeData.TemporaryAddressNp = obj.TemporaryAddressNp;
            _employeeData.TwoPunch = obj.TwoPunch;
            _employeeData.UserId = emp.UserId;
            _employeeData.WOTypeId = obj.WOTypeId;
        }


        private void PrepareData()
        {
            //_employeeData = new EEmployee();
            #region Office Record
            _employeeData.BranchId = RiddhaSession.BranchId;
            _employeeData.Code = txtEmployeeCode.Text;
            _employeeData.Name = txtEmpName.Text;
            _employeeData.DeviceCode = txtEmpDeviceCode.Text.ToInt();
            _employeeData.Designation = null;
            _employeeData.DesignationId = cmbDesignation.SelectedValue.ToInt();
            _employeeData.Section = null;
            _employeeData.SectionId = cmbSection.SelectedValue.ToInt();
            _employeeData.GradeGroup = null;
            _employeeData.GradeGroupId = cmbGradeGroup.SelectedValue.ToInt();
            _employeeData.IsManager = chkIsManager.Checked;
            #endregion


            #region Bassic Info
            _employeeData.MaritialStatus = (MaritialStatus)Enum.ToObject(typeof(MaritialStatus), cmbMaritalStatus.SelectedIndex);
            _employeeData.Gender = (Gender)Enum.ToObject(typeof(Gender), cmbGender.SelectedIndex);
            _employeeData.BloodGroup = (BloodGroup)Enum.ToObject(typeof(BloodGroup), cmbBloodGroup.SelectedIndex);
            _employeeData.Mobile = txtMobileNo.Text;
            _employeeData.Email = txtEmail.Text;
            _employeeData.PassportNo = txtPasswordNo.Text;
            _employeeData.CitizenNo = txtCitizenNumber.Text;
            _employeeData.IssueDistict = txtIssueDistrict.Text;
            _employeeData.Religion = (Religious)Enum.ToObject(typeof(Religious), cmbReligion.SelectedIndex);
            _employeeData.PermanentAddress = txtPermanentAddress.Text;
            _employeeData.TemporaryAddress = txtTemproryAddress.Text;
            #endregion

            #region Time Punch Management
            _employeeData.MaxWorkingHour = mtbMaxWorkingHour.Text.ToTimeSpan();
            _employeeData.AllowedLateIn = mtbAllowLateIn.Text.ToTimeSpan();
            _employeeData.AllowedEarlyOut = mtbAllowEarlyOut.Text.ToTimeSpan();
            _employeeData.HalfdayWorkingHour = mtbHalifDayWorkingHour.Text.ToTimeSpan();
            _employeeData.ShortDayWorkingHour = mtbShortDayWorkingHour.Text.ToTimeSpan();
            _employeeData.PresentMarkingDuration = mtbPresentMarkingDuration.Text.ToTimeSpan();
            _employeeData.MaxOTHour = mtbMaxOtHour.Text.ToTimeSpan();
            _employeeData.NoPunch = rbtnNoPunch.Checked;
            _employeeData.SinglePunch = rbtnSinglePunch.Checked;
            _employeeData.MultiplePunch = rbtnMultiplePunch.Checked;
            _employeeData.TwoPunch = rbtnTwoPunch.Checked;
            _employeeData.FourPunch = rbtnFourPunch.Checked;
            _employeeData.ConsiderTimeLoss = chkConsiderTimeLoss.Checked;
            _employeeData.HalfDayMarking = chkHalfDayMarking.Checked;
            _employeeData.IsOTAllowed = chkIsOtAllowed.Checked;
            _employeeData.MinOTHour = mtbMinOtHour.Text.ToTimeSpan();
            #endregion

            #region shift Wo Management
            _employeeData.ShiftTypeId = cmbShiftType.SelectedIndex;

            #endregion


            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    _employeeData.DateOfBirth = (mtbDateOfBirth.Text).ToNullableDatetime();
                    _employeeData.DateOfJoin = (mtbDateOfJoin.Text).ToNullableDatetime();
                    _employeeData.IssueDate = mtbIssueDate.Text.ToNullableDatetime();
                    break;
                case OperationDate.Nepali:
                    _employeeData.DateOfBirth = (mtbDateOfBirth.Text).ToEnglishDate();
                    _employeeData.DateOfJoin = (mtbDateOfJoin.Text).ToEnglishDate();
                    _employeeData.IssueDate = mtbIssueDate.Text.ToEnglishDate();
                    break;
                default:
                    break;
            }

            if (pcbPhoto.BackgroundImage != null)
            {
                var dir = currentDirectory + "\\images";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var imagePath = dir + @"\" + Guid.NewGuid() + "." + imageExt;
                pcbPhoto.BackgroundImage.Save(imagePath);
                _employeeData.ImageUrl = imagePath;
                //model = ;
            }
            //if (mtbDateOfJoin.Text == "  /  /")
            //{
            //    _employeeData.DateOfJoin = null;
            //}
            //if (mtbDateOfBirth.Text == "  /  /")
            //{
            //    _employeeData.DateOfBirth = null;
            //}
            //if (mtbIssueDate.Text == "  /  /")
            //{
            //    _employeeData.IssueDate = null;
            //}
            if (cmbGradeGroup.SelectedValue.ToInt() == 0)
            {
                _employeeData.GradeGroupId = null;
            }
        }

        string currentDirectory = System.IO.Directory.GetCurrentDirectory();
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files(*.jpg;*.png, *.jpeg; *.gif; *.bmp)|*.JPG;*.PNG; *.JPEG; *.GIF; *.BMP";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pcbPhoto.BackgroundImage = new Bitmap(openFileDialog1.FileName);
                pcbPhoto.BackgroundImageLayout = ImageLayout.Stretch;
                imageExt = openFileDialog1.SafeFileName.Split('.')[1];
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (employeeGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to edit.");
                return;
            }
            setSelectedData();
            if (_employeeData.Id == 0)
            {
                MessageBox.Show("Please select row to edit.");
                return;
            }
            setInputValue();
            btnSave.Text = "Update";
        }


        private void setSelectedData()
        {
            SEmployee employeeServices = new SEmployee();
            var selectedRow = employeeGridView.Rows[employeeGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as EmployeeGridVm;
            var selectedEmployee = employeeServices.List().Data.Where(x => x.Id == selectedData.Id).FirstOrDefault();
            _employeeData = new EEmployee();
            _employeeData = selectedEmployee;
        }
        private void setInputValue()
        {
            SEmployee employeeServices = new SEmployee();
            #region Office Record
            txtEmployeeCode.Text = _employeeData.Code;
            txtEmpName.Text = _employeeData.Name;
            txtEmpDeviceCode.Text = _employeeData.DeviceCode.ToString();
            cmbDesignation.SelectedValue = _employeeData.DesignationId == null ? 0 : _employeeData.DesignationId;
            cmbGradeGroup.SelectedValue = _employeeData.GradeGroupId == null ? 0 : _employeeData.GradeGroupId;
            if (_employeeData.SectionId != null)
            {
                cmbSection.SelectedValue = (_employeeData.SectionId);
                cmbDepartment.SelectedValue = _employeeData.Section == null ? 0 : _employeeData.Section.Department.Id;
            }
            chkIsManager.Checked = _employeeData.IsManager;
            #endregion

            #region Basic Info
            cmbMaritalStatus.SelectedIndex = (int)_employeeData.MaritialStatus;
            cmbGender.SelectedIndex = (int)_employeeData.Gender;
            cmbBloodGroup.SelectedIndex = (int)_employeeData.BloodGroup;
            txtMobileNo.Text = _employeeData.Mobile;
            txtEmail.Text = _employeeData.Email;
            txtPasswordNo.Text = _employeeData.PassportNo;
            txtCitizenNumber.Text = _employeeData.CitizenNo;

            cmbReligion.SelectedIndex = (int)_employeeData.Religion;
            if (File.Exists(_employeeData.ImageUrl))
            {
                var stream = new FileStream(_employeeData.ImageUrl, FileMode.Open, FileAccess.Read);
                pcbPhoto.BackgroundImage = Image.FromStream(stream);
            }
            else
            {
                pcbPhoto.Image = Riddhasoft.EHajiriPro.Desktop.Properties.Resources.men3;
            }
            pcbPhoto.BackgroundImageLayout = ImageLayout.Stretch;
            txtPermanentAddress.Text = _employeeData.PermanentAddress;
            txtTemproryAddress.Text = _employeeData.TemporaryAddress;
            #endregion

            #region TimePunch
            mtbMaxWorkingHour.Text = _employeeData.MaxWorkingHour.ToString(@"hh\:mm");
            mtbAllowLateIn.Text = _employeeData.AllowedLateIn.ToString(@"hh\:mm");
            mtbAllowEarlyOut.Text = _employeeData.AllowedEarlyOut.ToString(@"hh\:mm");
            mtbHalifDayWorkingHour.Text = _employeeData.HalfdayWorkingHour.ToString(@"hh\:mm");
            mtbShortDayWorkingHour.Text = _employeeData.ShortDayWorkingHour.ToString(@"hh\:mm");
            mtbMaxOtHour.Text = _employeeData.MaxOTHour.ToString(@"hh\:mm");
            mtbMinOtHour.Text = _employeeData.MinOTHour.ToString(@"hh\:mm");
            rbtnNoPunch.Checked = _employeeData.NoPunch;
            rbtnSinglePunch.Checked = _employeeData.SinglePunch;
            rbtnMultiplePunch.Checked = _employeeData.MultiplePunch;
            rbtnTwoPunch.Checked = _employeeData.TwoPunch;
            rbtnFourPunch.Checked = _employeeData.FourPunch;
            chkConsiderTimeLoss.Checked = _employeeData.ConsiderTimeLoss;
            chkHalfDayMarking.Checked = _employeeData.HalfDayMarking;
            chkIsOtAllowed.Checked = _employeeData.IsOTAllowed;
            #endregion

            switch (GlobalParam.OperationDate)
            {
                case OperationDate.English:
                    mtbIssueDate.Text = _employeeData.IssueDate.ToString();
                    mtbDateOfJoin.Text = _employeeData.DateOfJoin.ToString();
                    mtbDateOfBirth.Text = _employeeData.DateOfBirth.ToString();
                    break;
                case OperationDate.Nepali:
                    if (_employeeData.DateOfJoin != null)
                    {
                        mtbDateOfJoin.Text = NepaliDateExtension.ToNepaliDate((DateTime)_employeeData.DateOfJoin);
                    }
                    if (_employeeData.DateOfBirth != null)
                    {
                        mtbDateOfBirth.Text = NepaliDateExtension.ToNepaliDate((DateTime)_employeeData.DateOfBirth);
                    }
                    if (_employeeData.IssueDate != null)
                    {
                        mtbIssueDate.Text = NepaliDateExtension.ToNepaliDate((DateTime)_employeeData.IssueDate);
                    }
                    break;
                default:
                    break;
            }
            cmbShiftType.SelectedIndex = _employeeData.ShiftTypeId == null ? 0 : (int)_employeeData.ShiftTypeId;
            EEmployeeShitList empshift = employeeServices.ListEmpShift().Where(x => x.EmployeeId == _employeeData.Id).FirstOrDefault();
            var weeklyOff = employeeServices.ListEmpWeeklyOff(_employeeData.Id);
            cmbShift.SelectedValue = empshift == null ? 0 : empshift.ShiftId;
            foreach (var item in weeklyOff)
            {
                if (item.OffDayId == 0)
                {
                    chkWeeklyOffSunday.Checked = true;
                }
                else if (item.OffDayId == 1)
                {
                    chkWeeklyOffMonday.Checked = true;
                }
                else if (item.OffDayId == 2)
                {
                    chkWeeklyOffTuesday.Checked = true;
                }
                else if (item.OffDayId == 3)
                {
                    chkWeeklyOffWednesday.Checked = true;
                }
                else if (item.OffDayId == 4)
                {
                    chkWeeklyOffThursday.Checked = true;
                }
                else if (item.OffDayId == 5)
                {
                    chkWeeklyOffFriday.Checked = true;
                }
                else
                {
                    chkWeeklyOffSaturday.Checked = true;
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var selectedRow = employeeGridView.Rows[employeeGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as EmployeeGridVm;
            EmployeeViewDetailsFrm frm = new EmployeeViewDetailsFrm(selectedData.Id);
            frm.ShowDialog();
        }

        private void EmployeeFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SEmployee empServices = new SEmployee();
            var selectedRow = employeeGridView.Rows[employeeGridView.CurrentRow.Index];
            var selectedData = selectedRow.DataBoundItem as EmployeeGridVm;

            setSelectedData();

            if (employeeGridView.Rows.Count == 0)
            {
                MessageBox.Show("There is not any data to delete.", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_employeeData.Id == 0)
            {
                MessageBox.Show("Please select row to delete.", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Confirm to delete?", "Comfirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var emp = empServices.List().Data.Where(x => x.Id == selectedData.Id).FirstOrDefault();
                var result = empServices.Remove(emp);
                processResult<int>(result);
                MessageBox.Show("Remove Sucessfully", "Sucess",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            SEmployee employeeServices = new SEmployee();
            SUser userServices = new SUser();
            var user = userServices.List().Data.ToList();
            var data = (from c in employeeServices.List().Data.Where(x => x.Name.StartsWith(txtSearch.Text)).ToList()
                        select new EmployeeGridVm()
                        {
                            Id = c.Id,
                            Department = c.Section == null ? "" : c.Section.Department.Name,
                            Section = c.Section == null ? "" : c.Section.Name,
                            Designation = c.Designation == null ? "" : c.Designation.Name,
                            EmployeeCode = c.Code,
                            EmployeeName = c.Name,
                            ShiftType = c.ShiftTypeId == 0 ? "Fixed" : c.ShiftTypeId == 1 ? "Weekly" : "Monthly",
                            UserId = c.UserId
                        }).ToList();
            data.ForEach(x => x.LoginStatus = getLoginStatus(user, x.UserId) == true ? "Off" : "On");
            employeeGridView.DataSource = data;
        }
    }
}
