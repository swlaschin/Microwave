(*** hide ***)
namespace Microwave

module rec Domain =

(**
# Microwave domain

This domain is all about using a Microwave oven.

The oven has:

* a keypad for entering times
* a start button
* a button to open the door
*)

(**
## Common

Common types used throughout the domain
*)

    /// A placeholder until we know more
    type UserId = string

    // We create unit of measures to
    // (a) help with documentation and
    // (b) avoid errors caused by mixing up seconds and milliseconds.

    // NOTE: All the SI Units are available in F# in the FSharp.Data.UnitSystems.SI namespace

    type [<Measure>] second
    type [<Measure>] millisecond


    // We create a special `TimeRemaining` type rather than using a simple `int` because
    // (a) `int` is not a word in the domain and
    // (b) it will have constraints (e.g. must be > 0)

    type TimeRemaining = TimeRemaining of int<second>

(**
## Commands

Commands to operate the microwave
*)
    /// the complete list of commands
    type Command =
        | Open of OpenCommand
        | Close of CloseCommand
        | Start of StartCommand

    // information associated with the Open command
    type OpenCommand = {
        User : UserId
        }

    // information associated with the Close command
    type CloseCommand = {
        User : UserId
        }

    // information associated with the Start command
    type StartCommand = {
        User : UserId
        HowLong : TimeRemaining
        }

(**
## States

States the microwave can be in.

Business rules:

* The microwave cannot be running if the door is open
* The microwave cannot be running if the TimeRemaining = 0

*)

    type State =
        | DoorOpenIdle of DoorOpenIdleState
        | DoorClosedIdle of DoorClosedIdleState
        | Running of RunningState
        | DoorOpenPaused of DoorOpenPausedState

    /// A place holder for state associated with the door being open.
    type DoorOpenIdleState = DoorOpenIdleState
    type DoorClosedIdleState = DoorClosedIdleState

    /// The running state needs to keep track of the time remaining
    type RunningState = {
        TimeRemaining : TimeRemaining
        }

    /// The paused state also needs to keep track of the time remaining
    type DoorOpenPausedState = {
        TimeRemaining : TimeRemaining
        }

(**
## State transitions

Here, we document how to go from one state of the microwave to another.
E.g. opening a door, starting it, etc.
*)

    type OpenWhenIdle =
        OpenCommand -> DoorClosedIdleState -> DoorOpenIdleState

    type CloseWhenIdle =
        CloseCommand -> DoorOpenIdleState -> DoorClosedIdleState

    type Start =
        StartCommand -> DoorClosedIdleState -> RunningState

    type OpenWhenRunning =
        OpenCommand -> RunningState -> DoorOpenPausedState

    type CloseWhenRunning =
        CloseCommand -> DoorOpenPausedState -> RunningState



