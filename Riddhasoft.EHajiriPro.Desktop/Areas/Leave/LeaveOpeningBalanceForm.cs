using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riddhasoft.EHajiriPro.Desktop.Areas.Leave
{
    public partial class LeaveOpeningBalanceForm : Form
    {
        public LeaveOpeningBalanceForm()
        {
            InitializeComponent();
        }

        private void LeaveOpeningBalanceForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
