﻿namespace Quanta;

public class NumberButton : Button
{
    public bool IsDigit = true;

    private MainScene mainScene;

    public NumberButton()
    {
        Size = new(75, 60);
        //Size = new(60, 60);
        Style.Roundness = 0.5f;
        Style.FontSize = 32;
    }

    public override void Start()
    {
        mainScene = GetNode<MainScene>("/root");

        LeftClicked += OnLeftClicked;

        base.Start();
    }

    private void OnLeftClicked(object? sender, EventArgs e)
    {
        if (IsDigit)
        {
            mainScene.GetNode<MathTextBox>("MathTextBox").Insert(Text);
        }
        else
        {
            mainScene.GetNode<MathTextBox>("MathTextBox").Evaluate();
        }
    }
}