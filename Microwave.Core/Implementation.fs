namespace Microwave

module Implementation =
    open Domain


    let openWhenIdle : OpenWhenIdle =
        fun cmd currentState ->
            printfn "Door opened by %s" cmd.User
            let newStateInfo = DoorOpenIdleState
            newStateInfo

    let closeWhenIdle : CloseWhenIdle =
        fun cmd currentState ->
            printfn "Door closed by %s" cmd.User
            let newStateInfo = DoorClosedIdleState
            newStateInfo

    let start : Start =
        fun cmd currentState ->
            printfn "Started by %s " cmd.User
            let newStateInfo : RunningState =  // use a type annotation here to not mix up with DoorOpenPausedState
                {TimeRemaining = cmd.HowLong}
            newStateInfo

    let openWhenRunning : OpenWhenRunning =
        fun cmd currentState ->
            printfn "Door opened by %s when running" cmd.User
            let newStateInfo : DoorOpenPausedState = // use a type annotation here to not mix up with RunningState
                {TimeRemaining = currentState.TimeRemaining }
            newStateInfo

    let closeWhenPaused : CloseWhenRunning =
        fun cmd currentState ->
            printfn "Door closed by %s when running" cmd.User
            let newStateInfo : RunningState =
                {TimeRemaining = currentState.TimeRemaining }
            newStateInfo

