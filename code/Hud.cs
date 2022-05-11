using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Linq;

namespace BoxCrash
{
	class Hud : Sandbox.HudEntity<RootPanel>
	{
		public Hud()
		{
			if ( !IsClient )
				return;

			RootPanel.StyleSheet.Load( "/Hud.scss" );
			RootPanel.AddChild<BoxCounter>( "panel" );
			RootPanel.AddChild<HelpPanel>( "panel" );
		}
	}

	class BoxCounter : Panel
	{
		public Label BoxesLabel;
		public Label ResetsLabel;
		public Label TotalDeletedLabel;

		public BoxCounter()
		{
			BoxesLabel = Add.Label( "0", "value" );
			ResetsLabel = Add.Label( "0", "value" );
			TotalDeletedLabel = Add.Label( "0", "value" );
		}

		public override void Tick()
		{
			var game = BoxCrashGame.Instance;
			var boxes = Entity.All.OfType<Box>();

			BoxesLabel.Text = $"Boxes: {boxes.Count()}";
			ResetsLabel.Text = $"Resets: {game.BoxResets}";
			TotalDeletedLabel.Text = $"Total Deleted: {game.TotalDeletedBoxes}";
		}
	}

	class HelpPanel : Panel
	{
		public Label ShootSingleLabel;
		public Label ShootBlastLabel;
		public Label ResetLabel;

		public HelpPanel()
		{
			ShootBlastLabel = Add.Label( $"{Input.GetButtonOrigin( BoxCrashGame.ShootBlastButton ).ToUpper()} to shoot a blast of boxes" );
			ShootSingleLabel = Add.Label( $"{Input.GetButtonOrigin( BoxCrashGame.ShootSingleButton ).ToUpper()} to shoot a huge stream of boxes" );
			ResetLabel = Add.Label( $"{Input.GetButtonOrigin( BoxCrashGame.ResetButton ).ToUpper()} to reset" );
		}
	}
}
