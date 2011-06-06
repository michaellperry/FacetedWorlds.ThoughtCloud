using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UpdateControls.XAML.Wrapper;
using ThoughtCloud_Presentation.ViewModels;

namespace ThoughtCloud_Presentation
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                IPresentationViewModel viewModel = CurrentViewModel;
                if (viewModel != null)
                    viewModel.Forward();
            }
            else if (e.Key == Key.Left)
            {
                IPresentationViewModel viewModel = CurrentViewModel;
                if (viewModel != null)
                    viewModel.Backward();
            }
        }

        private IPresentationViewModel CurrentViewModel
        {
            get
            {
                UserControl view = LayoutRoot.Children.FirstOrDefault() as UserControl;
                if (view != null)
                {
                    IObjectInstance viewModel = view.DataContext as IObjectInstance;
                    if (viewModel != null)
                    {
                        return viewModel.WrappedObject as IPresentationViewModel;
                    }
                }
                return null;
            }
        }
	}
}