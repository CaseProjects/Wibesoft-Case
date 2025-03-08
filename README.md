# Wibesoft Case
https://github.com/user-attachments/assets/9c35de84-c0dc-4b66-a34d-c40fb2560c1a
 ## Unity 2022.3.18f1/Android Platform

### Project Structure

- #### The project is primarily built on Extenject. Dependencies of project references in scenes have been moved from the scene to a ScriptableObjectInstaller. This allows references to be distributed independently from a single source.
- #### Zenject's Signals system was used to make UI operations (such as button clicks and popup openings) event-driven. This ensures that senders and receivers communicate only through the signal, effectively decoupling dependencies.
- #### Since the game is heavily UI-based, the R3 (UniRx) Reactive Programming framework was used to simplify event management. Additionally, a custom R3 trigger was implemented to detect and close popups when clicking outside of them. This allows any desired popup to subscribe to and listen for this trigger.

https://github.com/CaseProjects/Wibesoft-Case/blob/834478db2ba636487e2b87d5e761ddbe6f938d7c/Assets/_Project/Scripts/Helpers/R3%20Triggers/ObservableClickOutsideTrigger.cs#L1-L31
- #### The UI infrastructure was designed to be flexible rather than static. This allows the UI to automatically update itself when new items are added, as long as the necessary prefabs and scriptable objects are provided.

## 3rd Party Assets
- ### Extenject(Zenject)
- ### R3(UniRx)
- ### UniTask






