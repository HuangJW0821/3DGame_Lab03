using UnityEngine;

public class CalculatorController : MonoBehaviour
{
    private CalculatorModel model;
    private CalculatorView view;

    void Start(){
        model = new CalculatorModel();
        view = new CalculatorView();
    }

    void OnGUI(){
        view.DrawCalculator(OnOperatorPressed, OnInputChanged, model.result.ToString(), model.errorMessage);
    }

    private void OnOperatorPressed(string operation){
        view.showError = false;
        model.operation = operation;

        if (!model.Calculate()){
            view.ShowError(model.errorMessage);
        }
    }

    private void OnInputChanged(string input){
        model.input1 = view.GetInput1();
        model.input2 = view.GetInput2();
    }
}

public class CalculatorModel
{
    public string input1;
    public string input2;
    public float result;
    public string operation;
    public string errorMessage;

    public CalculatorModel(){
        Clear();
    }

    public void Clear(){
        input1 = "0";
        input2 = "0";
        result = 0f;
        operation = "";
        errorMessage = "";
    }

    public bool Calculate(){
        if(!float.TryParse(input1, out float num1) || !float.TryParse(input2, out float num2)){
            errorMessage = "Invalid Input!";
            return false;
        }

        switch (operation){
            case "+":
                result = num1 + num2;
                break;
            case "-":
                result = num1 - num2;
                break;
            case "*":
                result = num1 * num2;
                break;
            case "/":
                if (num2 != 0)
                    result = num1 / num2;
                else
                {
                    errorMessage = "Cannot divide by zero!";
                    return false;
                }
                break;
            default:
                errorMessage = "Invalid operation!";
                return false;
        }
        return true;
    }
}

public class CalculatorView{
    private float calculatorWidth = 220f;
    private float calculatorHeight = 300f;
    private string input1 = "0";
    private string input2 = "0";
    public bool showError = false;

    public void DrawCalculator(System.Action<string> onOperatorPressed, System.Action<string> onInputChanged, string result, string errorMessage)
    {
        float centerX = (Screen.width - calculatorWidth) / 2;
        float centerY = (Screen.height - calculatorHeight) / 2;

        GUI.Box(new Rect(centerX, centerY, calculatorWidth, calculatorHeight), "Calculator");

        GUI.Label(new Rect(centerX + 10, centerY + 30, 200, 30), "Input 1:");
        input1 = GUI.TextField(new Rect(centerX + 10, centerY + 60, 200, 30), input1);
        onInputChanged(input1);

        GUI.Label(new Rect(centerX + 10, centerY + 100, 200, 30), "Input 2:");
        input2 = GUI.TextField(new Rect(centerX + 10, centerY + 130, 200, 30), input2);
        onInputChanged(input2);

        if (GUI.Button(new Rect(centerX + 10, centerY + 170, 40, 40), "+")) onOperatorPressed("+");
        if (GUI.Button(new Rect(centerX + 60, centerY + 170, 40, 40), "-")) onOperatorPressed("-");
        if (GUI.Button(new Rect(centerX + 110, centerY + 170, 40, 40), "*")) onOperatorPressed("*");
        if (GUI.Button(new Rect(centerX + 160, centerY + 170, 40, 40), "/")) onOperatorPressed("/");

        GUI.Label(new Rect(centerX + 10, centerY + 220, 200, 30), "Result:");

        if (showError)
        {
            GUI.Label(new Rect(centerX + 10, centerY + 250, 200, 30), errorMessage);
        }else{
            GUI.Label(new Rect(centerX + 10, centerY + 250, 200, 30), result);
        }
    }

    public void ShowError(string errorMessage){ showError = true;}

    public string GetInput1() => input1;
    public string GetInput2() => input2;
}
