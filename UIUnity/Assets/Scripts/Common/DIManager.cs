using Common.Interface;
using Common.Scriptable;
using Controller;
using UnityEngine;
using Zenject;

namespace Common
{
    public class DIManager : MonoInstaller
    {
        [SerializeField] private LoginInfoModel _loginInfoModel;
    
        public override void InstallBindings()
        {
            Container.BindInstance(_loginInfoModel).AsSingle().NonLazy();
            Container.Bind<ILoginController>().To<LoginController>().AsSingle().NonLazy();
            Container.Bind<INoteController>().To<NoteController>().AsSingle().NonLazy();
            
            // Container.Bind<ISubject<ViewMessage>>().FromInstance(new Subject<ViewMessage>()).AsSingle().NonLazy();
            Container.Bind<IBrokerMessage>().To<BrokerMessage>().AsSingle().NonLazy(); 
            //.FromInstance(new Subject<ViewMessage>()).AsSingle().NonLazy();
        }
    }
}
