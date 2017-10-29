using UnityEngine;
using Zenject;

public class MovementInstaller : MonoInstaller<MovementInstaller>
{
	public PlayerMovement.Settings PlayerMovementSettings;
	public PlayerInputHandler.Settings PlayerInputSettings;

	public Rigidbody2D PlayerRigidbody;

	public ParticleSystem PlayerParticles;
	
    public override void InstallBindings()
    {
		Container.Bind<PlayerMovement> ().AsSingle ();
		
		Container.Bind<PlayerMovement.Settings> ().FromInstance (PlayerMovementSettings).AsSingle ();
		Container.Bind<PlayerInputHandler.Settings> ().FromInstance (PlayerInputSettings).AsSingle ();

		Container.Bind<Rigidbody2D> ().WithId ("Player").FromInstance(PlayerRigidbody).AsSingle();
		Container.Bind<InputHandler> ().WithId ("Player").To<PlayerInputHandler>().AsSingle();
		Container.Bind<ParticleSystem> ().WithId ("Player").FromInstance (PlayerParticles).AsSingle ();

		Container.BindInterfacesTo<InputHandler> ().AsSingle ();
		Container.BindInterfacesTo<PlayerInputHandler> ().AsSingle ();
    }
}