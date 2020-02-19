using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignalRChatBasic.Views
{

    public partial class ChatPage : ContentPage
    {
        HubConnection connection;
        ObservableCollection<string> messages = new ObservableCollection<string>();
        public ChatPage()
        {
            InitializeComponent();

            // Connects to backend - "Hub" class route
            connection = new HubConnectionBuilder()
                .WithUrl(serverEntry.Text + "/chatHub")
                .Build();

            messagesList.ItemsSource = messages;
        }

        private async void ConnectButton_Clicked(object sender, EventArgs e)
        {
            // Handle receiving new messages
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                this.Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    var newMessage = $"{user}: {message}";
                    messages.Add(newMessage);
                });
            });

            // try to connect!
            try
            {
                await connection.StartAsync();
                messages.Add("Starting connection!!!!");
            }
            catch (Exception ex)
            {
                messages.Add(ex.Message);
            }
        }

        private async void SendButton_Clicked(object sender, EventArgs e)
        {
            // Use the "SendMessage" hub method
            try
            {
                await connection.InvokeAsync("SendMessage",
                    nameEntry.Text, messageEntry.Text);
            }
            catch (Exception ex)
            {
                messages.Add(ex.Message);
            }
        }
    }
}