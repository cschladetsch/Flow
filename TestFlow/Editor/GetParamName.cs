using System;
using System.Linq.Expressions;

public static class SymbolName
{
	public static string Get<T>(Expression<Func<T>> memberExpression)
	{
		return ((MemberExpression)memberExpression.Body).Member.Name;
	}
}

/*

To get name of a variable:

string testVariable = "value";
string nameOfTestVariable = SymbolName.Get(() => testVariable);


To get name of a parameter:

public class TestClass
{
    public void TestMethod(string param1, string param2)
    {
        string nameOfParam1 = SymbolName.Get(() => param1);
    }
}

	*/
