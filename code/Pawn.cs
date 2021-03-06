using Sandbox;
using System;
using System.Linq;

namespace BoxCrash
{
	partial class Pawn : AnimEntity
	{
		/// <summary>
		/// Called when the entity is first created 
		/// </summary>
		public override void Spawn()
		{
			base.Spawn();

			//
			// Use a watermelon model
			//
			SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );

			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			Rotation = Input.Rotation;
			EyeRotation = Rotation;

			// build movement from the input values
			var movement = new Vector3( Input.Forward, Input.Left, 0 ).Normal;

			// rotate it to the direction we're facing
			Velocity = Rotation * movement;

			// apply some speed to it
			Velocity *= Input.Down( InputButton.Run ) ? 1000 : 200;

			// apply it to our position using MoveHelper, which handles collision
			// detection and sliding across surfaces for us
			MoveHelper helper = new MoveHelper( Position, Velocity );
			helper.Trace = helper.Trace.Size( 16 );
			if ( helper.TryMove( Time.Delta ) > 0 )
			{
				Position = helper.Position;
			}

			if ( IsServer )
			{
				if (Input.Down( BoxCrashGame.ShootStreamButton ) )
				{
					ShootBoxes();
				}
				else if ( Input.Pressed( BoxCrashGame.ShootBlastButton ) )
				{
					ShootBoxes();
				}

				if ( Input.Pressed( BoxCrashGame.ResetButton ) )
				{
					BoxCrashGame.Instance.ResetBoxes();
				}
			}
		}

		private void ShootBoxes()
		{
			for ( int i = 0; i < 10; i++ )
			{
				var box = new Box();
				box.Position = EyePosition + EyeRotation.Forward * 80;
				box.Rotation = Rotation.LookAt( Vector3.Random.Normal );
				box.Position -= box.Rotation * box.Model.Bounds.Center;
				box.PhysicsGroup.Velocity = EyeRotation.Forward * 1000;
			}
		}

		/// <summary>
		/// Called every frame on the client
		/// </summary>
		public override void FrameSimulate( Client cl )
		{
			base.FrameSimulate( cl );

			// Update rotation every frame, to keep things smooth
			Rotation = Input.Rotation;
			EyeRotation = Rotation;
		}
	}
}
