namespace Microwave

/// A module to implement the database access logic
module Database =
    open Domain

    /// Define a global mutable value to act as the "database".
    /// Note that the "mutable" keyword is needed as values are immutable by default.
    let mutable state =
        /// All values MUST be initialized. In this case, initialize it to the DoorClosedIdle state
        DoorClosedIdle (DoorClosedIdleState)

    let loadState() =
        state

    let saveState newState =
        state <- newState


module Api =
    open Domain

    // IMPORTANT: In F#, matching on each possible state is an *expression*, which means that each branch must return a value of the same type.
    // Instead, all branches must return the same thing, so we'll define a new type called `ApiResult`
    // which contains the state and a (possibly blank) error message.

    type ApiResult = {
        State : State
        Error : Error
        }

    let Open() =
        // do any I/O first
        let currentState = Database.loadState()
        // create the command. In a real app the user might come from the current IPrincipal, etc.
        let cmd : OpenCommand = {User = "Scott"}

        // for each possible state, handle the command
        match currentState with
        | DoorClosedIdle stateInfo ->
            let newStateInfo = Implementation.openWhenIdle cmd stateInfo
            let newState = DoorOpenIdle newStateInfo
            Database.saveState newState
            // return the new state and a blank error message
            {State = newState; Error = NoError}
        | DoorOpenIdle stateInfo ->
            let errMsg = CantOpenDoorWhenDoorIsAlreadyOpen
            // return the original state and an error message
            {State = currentState; Error = errMsg }
        | DoorOpenPaused stateInfo ->
            let errMsg = CantOpenDoorWhenDoorIsAlreadyOpen
            {State = currentState; Error = errMsg }
        | Running stateInfo ->
            let newStateInfo = Implementation.openWhenRunning cmd stateInfo
            let newState = DoorOpenPaused newStateInfo
            Database.saveState newState
            {State = newState; Error = NoError}

    let Close() =
        let currentState = Database.loadState()
        let cmd : CloseCommand = {User = "Scott"}
        match currentState with
        | DoorOpenIdle stateInfo ->
            let newStateInfo = Implementation.closeWhenIdle cmd stateInfo
            let newState = DoorClosedIdle newStateInfo
            Database.saveState newState
            {State = newState; Error = NoError}
        | DoorClosedIdle stateInfo ->
            let errMsg = CantCloseDoorWhenDoorIsAlreadyClosed
            {State = currentState; Error = errMsg }
        | Running stateInfo ->
            let errMsg = CantCloseDoorWhenDoorIsAlreadyClosed
            {State = currentState; Error = errMsg }
        | DoorOpenPaused stateInfo ->
            let newStateInfo = Implementation.closeWhenPaused cmd stateInfo
            let newState = Running newStateInfo
            Database.saveState newState
            {State = newState; Error = NoError}

    let Start(howLong) =
        let currentState = Database.loadState()
        let timeRemainingOpt = TimeRemaining.create howLong  // create a TimeRemaining value from the parameter passed in.
        match timeRemainingOpt with
        | None ->
            let errMsg = CantUseNegativeTimeRemaining
            {State = currentState; Error = errMsg }
        | Some timeRemaining ->
            let cmd : StartCommand = {User = "Scott"; HowLong = timeRemaining }
            match currentState with
            | DoorClosedIdle stateInfo ->
                let newStateInfo = Implementation.start cmd stateInfo
                let newState = Running newStateInfo
                Database.saveState newState
                {State = newState; Error = NoError}
            // we can match all the unhandled states together, like this
            | DoorOpenIdle _
            | DoorOpenPaused _
            | Running _ ->
                let errMsg = CantStart
                {State = currentState; Error = errMsg }

    let GetState() =
        let currentState = Database.loadState()
        currentState

    /// Convert the state into a string for display in the UI
    let StateToString(state) =
        // a simple and crude implementation
        sprintf "%A" state

    /// Convert the error into a string for display in the UI
    let ErrorToString(lang, error) =
        match error with
        | NoError ->
            ""
        | CantUseNegativeTimeRemaining ->
            "Can't Use Negative Time"
        | CantCloseDoorWhenDoorIsAlreadyClosed ->
            match lang with
            | "fr-FR" ->
                "Impossible de fermer la porte quand la porte est déjà fermée"
            | _ ->
                "Can't Close Door When Door Is Already Closed"
        | CantOpenDoorWhenDoorIsAlreadyOpen ->
            "Can't Open Door When Door Is Already Open"
        | CantStart ->
            "Can't Start"
