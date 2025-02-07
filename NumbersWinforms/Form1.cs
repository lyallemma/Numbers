// ReSharper disable once RedundantUsingDirective

using Humanizer;
using NumbersHelper;

namespace NumbersWinforms;

public partial class Form1 : Form
{
    private const string StillCountsAsAGuess = "This still counts as a guess";
    private NumbersLib _game = null!;

    public Form1()
    {
        InitializeComponent();
        SetupGame();
    }

    private void btnGuess_Click(object sender, EventArgs e)
    {
        MakeGuess();
        updateForfeitButton();
    }

    private void MakeGuess()
    {
        var result = _game.MakeGuess(txtGuess.Text);
        var resultMessage = string.Empty;
        switch (result)
        {
            case GuessResult.CorrectGuess:
                DisplayWinBanner();
                break;
            case GuessResult.Lower:
                resultMessage = "Lower...";
                break;
            case GuessResult.Higher:
                resultMessage = "Higher...";
                break;
            case GuessResult.PreviousGuess:
                resultMessage = $"Same as previous guess. {StillCountsAsAGuess}!";
                break;
            case GuessResult.PreviousGuessHigher:
                resultMessage = $"You've already guessed lower than {_game.CurrentGuess}. {StillCountsAsAGuess}.";
                break;
            case GuessResult.PreviousGuessLower:
                resultMessage = $"You've already guessed higher than {_game.CurrentGuess}. {StillCountsAsAGuess}.";
                break;
            case GuessResult.Forfeit:
                resultMessage = $"You forfeit. {_game.SecretNumber} was the right answer!";
                showPlayQuitButtons();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        DisplayRangeDetails();
        lblResult.Text = resultMessage;
        lblResult.ForeColor = result switch
        {
            GuessResult.Higher => Color.Red,
            GuessResult.Lower => Color.Green,
            GuessResult.PreviousGuessHigher => Color.DarkOrange,
            GuessResult.PreviousGuessLower => Color.DarkOrange,
            _ => Color.Black
        };

        lblResult.Visible = true;
    }

    private void DisplayRangeDetails()
    {
        lblRange.Text = $"Please guess a number between {_game.ThresholdLow} and {_game.ThresholdHigh}";
    }

    private void DisplayWinBanner()
    {
        lblWinBanner.Text = $"Congratulations! {_game.SecretNumber} is the right answer!";
        lblGuessCount.Text = $"It took you {"guess".ToQuantity(_game.NumberOfGuesses)} to win. Great game!";
        lblResult.Visible = false;
        lblGuessCount.Visible = true;
        lblWinBanner.Visible = true;
        showPlayQuitButtons();
    }

    private void btnForfeit_Click(object sender, EventArgs e)
    {
        txtGuess.Text = "0";
        MakeGuess();
    }

    private void btnPlayAgain_Click(object sender, EventArgs e)
    {
        SetupGame();
    }

    private void SetupGame()
    {
        _game = new NumbersLib();
        DisplayRangeDetails();
        lblResult.Visible = false;
        lblWinBanner.Visible = false;
        lblGuessCount.Visible = false;
        txtGuess.Text = string.Empty;
        hidePlayQuitButtons();
        btnForfeit.Enabled = false;
    }

    private void btnQuit_Click(object sender, EventArgs e)
    {
        Close();
    }
    
    private void updateForfeitButton()
    {
        if(_game.NumberOfGuesses > 0)
            btnForfeit.Enabled = true;
    }

    private void hidePlayQuitButtons(){
        btnPlayAgain.Visible = false;
        btnQuit.Visible = false;
    }

    private void showPlayQuitButtons(){
        btnPlayAgain.Visible = true;
        btnQuit.Visible = true;
    }
}