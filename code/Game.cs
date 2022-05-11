
using Sandbox;
using System;
using System.Linq;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace BoxCrash
{
	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class BoxCrashGame : Sandbox.Game
	{
		public static BoxCrashGame Instance => Current as BoxCrashGame;

		public static InputButton ShootBlastButton => InputButton.Attack1;
		public static InputButton ShootSingleButton => InputButton.Attack2;
		public static InputButton ResetButton => InputButton.Reload;

		[Net] public int BoxResets { get; private set; } = 0;
		[Net] public int TotalDeletedBoxes { get; private set; } = 0;

		public BoxCrashGame()
		{
			if ( IsServer )
			{
				_ = new Hud();
			}
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			// Create a pawn for this client to play with
			var pawn = new Pawn();
			client.Pawn = pawn;

			// Get all of the spawnpoints
			var spawnpoints = Entity.All.OfType<SpawnPoint>();

			// chose a random one
			var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

			// if it exists, place the pawn there
			if ( randomSpawnPoint != null )
			{
				var tx = randomSpawnPoint.Transform;
				tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
				pawn.Transform = tx;
			}
		}

		public void ResetBoxes()
		{
			var boxes = Entity.All.OfType<Box>();
			var count = boxes.Count();

			if ( count != 0 )
			{
				foreach ( var box in boxes )
				{
					box.Delete();
				}

				BoxResets++;
				TotalDeletedBoxes += count;

				Log.Trace( $"Reset {count} boxes [total: {TotalDeletedBoxes} boxes over {BoxResets} resets]" );
			}
		}
	}

}
