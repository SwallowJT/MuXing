using UnityEngine;
using System.Collections;

public class MediatorRegister {
    public static void Register()
    {
        UIManager manager = UIManager.Instance;
        
        //----For CreateMVC
		manager.RegisterMediator(new BattleMediator(ModulesLib.BATTLE), true);
		manager.RegisterMediator(new MainMediator(ModulesLib.MAIN), true);
		manager.RegisterMediator(new LoginMediator(ModulesLib.LOGIN), true);
    }
}
