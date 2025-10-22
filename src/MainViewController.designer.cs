// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

namespace CrispLEDES
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		AppKit.NSTableView MessageInfoView { get; set; }

		[Action ("ChooseConfigurationFile:")]
		partial void ChooseConfigurationFile (Foundation.NSObject sender);

		[Action ("ChooseCsvFiles:")]
		partial void ChooseCsvFiles (Foundation.NSObject sender);

		[Action ("CopySelectedRows:")]
		partial void CopySelectedRows (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (MessageInfoView != null) {
				MessageInfoView.Dispose ();
				MessageInfoView = null;
			}
		}
	}
}
