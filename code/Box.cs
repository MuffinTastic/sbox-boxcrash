using Sandbox;

namespace BoxCrash
{
	class Box : Sandbox.ModelEntity
	{
		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/citizen_props/crate01.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
		}
	}
}
