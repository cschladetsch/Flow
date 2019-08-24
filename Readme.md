# Flow ![Foo](dlow-small.jpg)
[![Build status](https://ci.appveyor.com/api/projects/status/github/cschladetsch/flow?svg=true)](https://ci.appveyor.com/project/cschladetsch/flow)
[![CodeFactor](https://www.codefactor.io/repository/github/cschladetsch/flow/badge)](https://www.codefactor.io/repository/github/cschladetsch/flow)
[![License](https://img.shields.io/github/license/cschladetsch/flow.svg?label=License&maxAge=86400)](./LICENSE.txt)
[![Release](https://img.shields.io/github/release/cschladetsch/flow.svg?label=Release&maxAge=60)](https://github.com/cschladetsch/flow/releases/latest)
[![Discord](https://discordapp.com/api/guilds/614846759075577882/widget.json)](https://discord.gg/SharedCodingTools)

A C# coroutine-based Kernel for .Net. If you are one of the many developers using this library, I encourage you provide any feedback and/or fork.

This is Unity-friendly and will work on all versions of Unity after 4.0. Please let me know otherwise.

Current documentation is at [GamaSutra](http://www.gamasutra.com/view/news/177397/Indepth_Flow__A_coroutine_kernel_for_Net.php) but the formatting is a bit screwy.

The original post was on [AltDevBlogADay](http://www.altdevblogaday.com/2012/09/07/flow-a-coroutine-kernel-for-net/) but that site is now lost for the ages.

## Tests

The [tests](TestFlow/Editor) reside in _TestFlow/Editor_ so they can be used from Unity3d as well.

## Example

This is a code example pulled straight for a game I'm working on:

```
public void GameLoop()
{
    Root.Add(
        New.Sequence(
            New.Coroutine(StartGame).Named("StartGame"),
            New.While(() => !_gameOver),
                New.Coroutine(PlayerTurn).Named("Turn").Named("While"),
            New.Coroutine(EndGame).Named("EndGame")
        ).Named("GameLoop")
    );
}
```
Note the `.Named("Name")` extenstions to the factory methods: these are for debugging and tracing purposes. The library comes with extensive debugging and visualisation support, so you can see in real time as the kernel changes.

The main logic _flow_ for starting a turn is:

```
private IEnumerator StartGame(IGenerator self)
{
    var start = New.Sequence(
        New.Barrier(
            WhitePlayer.StartGame(),
            BlackPlayer.StartGame()
        ).Named("Init Game"),
        New.Barrier(
            WhitePlayer.DrawInitialCards(),
            BlackPlayer.DrawInitialCards()
        ).Named("Deal Cards"),
        New.Barrier(
            New.TimedBarrier(
                TimeSpan.FromSeconds(Parameters.MulliganTimer),
                WhitePlayer.AcceptCards(),
                BlackPlayer.AcceptCards()
            ).Named("Mulligan"),
            New.Sequence(
                WhitePlayer.PlaceKing(),
                BlackPlayer.PlaceKing()
            ).Named("Place Kings")
        ).Named("Preceedings")
    ).Named("Start Game");
    start.Completed += (tr) => Info("StartGame completed");
    yield return start;
}
```

And the relevant IPlayerAgent Method declaractions as being:

```
ITimer StartGame();
ITimer DrawInitialCards();
ITimedFuture<bool> AcceptCards();
ITimedFuture<PlacePiece> PlaceKing();
ITransient ChangeMaxMana(int i);
ITimedFuture<ICardModel> DrawCard();
ITimedFuture<PlacePiece> PlayCard();
ITimedFuture<MovePiece> MovePiece();
ITimedFuture<Pass> Pass();
```

This is just a simple example on how the library is tyicall used. It's a matter of chainging together sequences of _Barriers_, _Triggers_, and _Futures_ to remove the need to keep explicit track of internal state on each *Update* call.

In this case, I'm using a lot of timed futures because it's a real-time card game and there are time limits.
