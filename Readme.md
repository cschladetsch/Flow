# Dekuple

[Christian Schladetsch](mailto:christian.schladetsch@gmail.com)

A dependancy-injection and entity system designed from the ground up to support both rapid prototyping and long-term development and support of `Unity3d` based applications.

The system also adds a `MVC` or `Model-View-Controller` pattern.

Except in this case, the `Controller` is called an `Agent` and uses the `Flow` library, and `Views` are based on `MonoBehavior`s.

The basic architecture is a morphism of a number ideas combined together into an integrated whole:
 * Dependancy Injection
 * Object Registry/Factory for persistence and networking
 * Model/View/Controller, or Model/ViewController/View
 * Unity3d Prefabs and Behaviours
 * Reactive programming techniques

The code is not 'tricky', other than the hoops required for templates in C#. The key hurdle a user will face is simply understanding the architecture and semantics of the seemingly simple systems.

One key understanding required is that one Registry\<T\> class is used for each model/agent/view domains, but of course with a different template parameter T.

There are Readme's in each substantial sub-folder that describes each component in more detail.

### Dependancies

This library uses the external [CoLib](http://www.github.com) and [Flow](https://www.github.com/cschladetsch/Flow) libraries. These are added as git sub-modules.

## Main Components

* [Registry](Registry/Readme.md)
* [Model](Model/Readme.md)
* [Agent](Agent/Readme.md)
* [View](View/Readme.md)

## Request/Response

Used for internal message-passing/queing and in future for networking when combined with the [Pyro](https://www.github.com/cschladetsch/Pyro) system.

## Future Work

This system is intended to be used with the new Unity3d ECS system.

Currently, only the View system has any reference to Unity3d.

It would be nice to separate that into a separate Assembly, so the system could be used outside the context of `Unity3d`.

### TODO

Make Error(..) etc log methods return object so they can return null and simplify usage.

