using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TipCalculatorProject
{
    public partial class MainPage : ContentPage
    {

        //for top display functionality 
        double billAmount = 0.00;
        double tipAmount = 0.00;
        double totalAmount = 0.00;
        double perPersonAmount = 0.00;

        //for calculator functionality
        bool hasStarted = false;
        bool hasTypedDecimal = false;
        int numberOfDecimalDigits = 0;

        public MainPage()
        {
            InitializeComponent();

            // under assumption that you can't start with 0 or .
            dotButton.IsEnabled = false;
            zeroButton.IsEnabled = false;


            //set percentage slider to 10% by default
            percentageSlider.Value = 10;
        }



        void updateUI()
        {

            tipAmount = Math.Round((billAmount * percentageSlider.Value) / 100, 2);
            totalAmount = billAmount + tipAmount;
            perPersonAmount = Math.Round(totalAmount / numberOfDinersStepper.Value, 2);


            billTotalLabel.Text = $" ${billAmount.ToString()}";
            tipTotalLabel.Text = $" ${tipAmount.ToString()}";
            totatTotalLabel.Text = $" ${totalAmount.ToString()}";
            splitTotalLabel.Text = $" ${perPersonAmount.ToString()}";
        }

        void percentageSlider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            percentageLabel.Text = $" {Math.Round(percentageSlider.Value).ToString()} %";


            updateUI();

        }

        private void CalculatorButtonTapped(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            // Testing to consol Console.WriteLine(button.Text);

            //reset if click 'C' is tapped

            if (button.Text == "C")
            {
                reset();
                return;
            }

            //checking if '0' is a valid key (another key must be pressed before 0 is active)

            if (button.Text == "0" && hasStarted == false)
            {
                return;
            }

            //start accepting input to billAmount

            if (hasStarted == false)
            {
                hasStarted = true;
                dotButton.IsEnabled = true;
                zeroButton.IsEnabled = true;
            }

            //decimal input, only allow 2 decimal points

            if (button.Text == ".")
            {
                hasTypedDecimal = true;

                return;
            }

            //accept all numbers
            else
            {
                if (hasTypedDecimal == true)
                {
                    if (numberOfDecimalDigits < 2)
                    {
                        numberOfDecimalDigits += 1;
                    }
                    else
                    {
                        DisplayAlert(" ", "You can only enter two decimal places", "OK");
                        return;
                    }
                }
            }

            //updating the bill amount value

            if (hasTypedDecimal == true)
            {
                //dealing with decimals. Calculate the amount to ad

                float multiplier = numberOfDecimalDigits == 2 ? 0.01F : 0.1F;
                billAmount += Math.Round(multiplier * Int32.Parse(button.Text), 2);


            }

            else
            {
                //dealing with all other numbers before the decimal point
                //it does this by shifting the number 'down' ie mulitply by 10

                billAmount *= 10;

                //convert the button text to int
                billAmount += Int32.Parse(button.Text);

            }

            updateUI();

        }

        private void reset()
        {
            hasStarted = false;
            hasTypedDecimal = false;
            numberOfDecimalDigits = 0;
            billAmount = 0.00;
            tipAmount = 0.00;
            totalAmount = 0.00;

            updateUI();

        }

        void numberOfDinersStepper_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {

            numberOfDinersLabel.Text = $" {numberOfDinersStepper.Value.ToString()} people";


            updateUI();
        }
    }

}
