namespace Main.SnoweveToolKit.EventSys.Example
{
    // public class TestSystem
    // {
    //     protected override void Initialize()
    //     {
    //         base.Initialize();
    //         SubscribeEvent();
    //     }
    //
    //     private IDisposable _disposable;
    //
    //     private void SubscribeEvent()
    //     {
    //         _disposable?.Dispose();
    //         var bag = DisposableBag.CreateBuilder();
    //         GlobalMessagePipe.GetSubscriber<>().Subscribe().AddTo(bag);
    //         _disposable = bag.Build();
    //     }
    //
    //     protected override void Release()
    //     {
    //         _disposable?.Dispose();
    //         base.Release();
    //     }
    // }
}