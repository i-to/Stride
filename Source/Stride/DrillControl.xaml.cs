using System.Windows.Controls;

namespace Stride
{
    public partial class DrillControl : UserControl
    {
        public DrillViewModel DrillViewModel { get; }
        
        public DrillControl(DrillViewModel drillViewModel)
        {
            DrillViewModel = drillViewModel;
            InitializeComponent();
        }
    }
}
