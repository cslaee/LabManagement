using System;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;



namespace LabManagement
{
    public partial class EmailCombinations : Form
    {
        StringBuilder diologText = new StringBuilder();
        StringBuilder emailText = new StringBuilder();

        public EmailCombinations()
        {
            InitializeComponent();
            ClearEmailContent();

        }

        private void add_Click(object sender, EventArgs e)
        {

            if (combination.Text == "")
                return;

            if (lockerNumber.Text == "")
                AppendText("Lock number " + lockNumber.Text + " with the combination of " + combination.Text);
            else
                AppendText("Locker number " + lockerNumber.Text + " with the combination of " + combination.Text);
            outGoingMessage.Text = diologText.ToString();
            Lock temp = new Lock(lockNumber.Text);

            Console.WriteLine("EmailCombinations.add_Click = " + temp.number + " = " + temp.cw1 + "-" + temp.ccw + "-" + temp.cw2);
            lockNumber.Text = "";
            lockerNumber.Text = "";


        }


        private void Button_Send(object sender, EventArgs e)
        {
            AppendText(" ");
            AppendText("Thank You,");
            AppendText(Constants.username);
            sendEMailThroughOUTLOOK();
            ClearEmailContent();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            ClearEmailContent();
        }


        private void AppendText(String inputText)
        {
            emailText.AppendLine("<br />" + inputText);
            diologText.AppendLine(inputText);

        }

        //method to send email to outlook
        public void sendEMailThroughOUTLOOK()

        {
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                //oMsg.HTMLBody = "<p>" + emailText.ToString() + "<p>"; 
                oMsg.HTMLBody = emailText.ToString();
                //Add an attachment.
                String sDisplayName = "MyAttachment";
                int iPosition = (int)oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                //now attached the file
                //Outlook.Attachment oAttach = oMsg.Attachments.Add(@"C:\\fileName.jpg", iAttachType, iPosition, sDisplayName);
                //Subject line
                oMsg.Subject = "Here is the combination for your EE locker";
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(Constants.email);
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
            }//end of try block
            catch (Exception ex)
            {
            }//end of catch
        }//end of Email Method

        private void EmailCombinations_Load(object sender, EventArgs e)
        {

        }



        private void lockNumber_TextChanged(object sender, EventArgs e)
        {
            Lock l = new Lock(lockNumber.Text);
            if (l.cw1 > 0)
                combination.Text = l.cw1 + "-" + l.ccw + "-" + l.cw2;
            else
                combination.Text = "";
        }

        void ClearEmailContent()
        {
            emailText.Clear();
            diologText.Clear();
            AppendText("Hi,");
            AppendText(" ");
            AppendText("Here is the combination to the locker that you have requested.");
            outGoingMessage.Text = diologText.ToString();
        }
    }
}
