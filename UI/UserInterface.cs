using System;
using System.Collections.Generic;

public class UserInterface
{
	public UserInterface()
	{
        clsPessoa user = new clsPessoa();
        List<clsPessoa> pessoa = user.GetAllTableData().Cast<clsPessoa>().ToList();
        var pessoaByPk = user.GetTableDataByPk("1").Cast<clsUserLogin>();

        clsPessoa p = new clsPessoa();
        p.Nome = "Nome1";
        p.Status = true;
        p.save();

        user.delete();
    }
}
