using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static AiHelloWorld.ChatBubble;

namespace AiHelloWorld
{

    public partial class Form1 : Form
    {
		private HttpClient client = new HttpClient();
		private List<ChatMessage> history = new List<ChatMessage>(); 
		private AudioRecorder _recorder;
		private string audiofilePath = "C:\\Users\\xuanz\\Downloads\\whisper\\wav\\input.wav";


		public Form1()
        {
            InitializeComponent(); 
			this.Shown += (s, e) => textBox2.Focus(); 
			_recorder = new AudioRecorder(audiofilePath);


			btnPushToTalk.MouseDown += BtnPushToTalk_MouseDown;
			btnPushToTalk.MouseUp += BtnPushToTalk_MouseUp;
			btnPushToTalk.MouseLeave += BtnPushToTalk_MouseLeave; // safety
			//WhisperServerStarts();
		}

        private void BtnPushToTalk_MouseLeave(object sender, EventArgs e)
        {
			if (Control.MouseButtons == MouseButtons.Left)
			{
				btnPushToTalk.Text = "Talk";
				StopAndProcessAsync();
			}

		}

		private void BtnPushToTalk_MouseUp(object sender, MouseEventArgs e)
        {
			btnPushToTalk.Text = "Talk";
			StopAndProcessAsync();

		}

		private void BtnPushToTalk_MouseDown(object sender, MouseEventArgs e)
        {
			btnPushToTalk.Text = "Listening...";
			_recorder.StartRecording();

		}

		private async Task StopAndProcessAsync()
		{
			_recorder.StopRecording();
			_recorder.Dispose();

			
			var input = WhisperTranscribe();
			ResponseMessgae(input);
		
		}

		private async void btnSend_Click(object sender, EventArgs e)
        {
			string input = textBox2.Text;
			textBox2.Text = "";

			ResponseMessgae(input);
		}

		private async void ResponseMessgae(string input)
		{
			if (input == "")
				return;
			history.Add(new ChatMessage { Role = "User", Content = input });
			AddMyMessage(input);

			string resultString = await DoWorkAsync();
			var result = JsonSerializer.Deserialize<LlamaResponse>(resultString);

			history.Add(new ChatMessage { Role = "Assistant", Content = result.Response });
			AddBotMessage(result.Response);
		} 

		private void AddMyMessage(string text)
		{
			var bubble = new ChatBubble();
			bubble.MessageText = text;
			bubble.SetAlignment(BubbleAlign.Right);

			flowLayoutPanel1.Controls.Add(bubble);
			flowLayoutPanel1.ScrollControlIntoView(bubble);
		}

		private void AddBotMessage(string text)
		{
			var bubble = new ChatBubble();
			bubble.MessageText = text;
			bubble.SetAlignment(BubbleAlign.Left);

			flowLayoutPanel1.Controls.Add(bubble);
			flowLayoutPanel1.ScrollControlIntoView(bubble);
		}

		private string BuildPrompt()
		{
			var sb = new StringBuilder();
			sb.AppendLine("You are a helpful assistant.");
			sb.AppendLine();

			foreach(var msg in history)
			{
				sb.AppendLine($"{msg.Role.ToUpper()}: {msg.Content}");
			}

			return sb.ToString();
		}

		private async Task<string> DoWorkAsync()
		{
			var payload = new
			{
				model = "llama3.2:3b",
				prompt = BuildPrompt(),
				stream = false
			};
			var json = JsonSerializer.Serialize(payload);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await client.PostAsync("http://localhost:11434/api/generate", content);
			var result = await response.Content.ReadAsStringAsync();

			return result;
		}

		/*private void WhisperServerStarts()
		{
			string whisperExe = "C:\\Users\\xuanz\\Downloads\\whisper\\whisper-server.exe";
			string modelPath = "C:\\Users\\xuanz\\Downloads\\whisper\\models\\ggml-small.en.bin";

			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = whisperExe,
					Arguments = $"-m \"{modelPath}\"",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};
			process.Start();
			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();
			process.WaitForExit();

			Console.WriteLine(error);
		}*/

		private string WhisperTranscribe()
		{
			string whisperExe = "C:\\Users\\xuanz\\Downloads\\whisper\\whisper-cli.exe";
			string modelPath = "C:\\Users\\xuanz\\Downloads\\whisper\\models\\ggml-small.en.bin";

			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = whisperExe,
					Arguments = $"-m \"{modelPath}\" -f \"{audiofilePath}\" --no-timestamps",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};
			process.Start();
			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();
			process.WaitForExit();

            Console.WriteLine(error);
			return output.Trim();
		}

		/*private async Task<string> WhisperTranscribeAsync()
		{
			var client1 = new HttpClient();
			var form = new MultipartFormDataContent();
			form.Add(new ByteArrayContent(File.ReadAllBytes(audiofilePath)), "file", "input.wav)");

			var response = await client1.PostAsync(
				"http://127.0.0.1:8080/inference", form);
			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<WhisperResponse>(json);

			return result?.text?.Trim();
		}*/

		public class WhisperResponse
		{
            public string text { get; set; }
        }

		public class LlamaResponse
		{
			[JsonPropertyName("model")]
			public string Model { get; set; }

			[JsonPropertyName("created_at")]
			public DateTime CreatedAt { get; set; }

			[JsonPropertyName("response")]
			public string Response { get; set; }

			[JsonPropertyName("done")]
			public bool Done { get; set; }

			[JsonPropertyName("done_reason")]
			public string DoneReason { get; set; }

			[JsonPropertyName("context")]
			public List<int> Context { get; set; }

			[JsonPropertyName("total_duration")]
			public long TotalDuration { get; set; }

			[JsonPropertyName("load_duration")]
			public long LoadDuration { get; set; }

			[JsonPropertyName("prompt_eval_count")]
			public int PromptEvalCount { get; set; }

			[JsonPropertyName("prompt_eval_duration")]
			public long PromptEvalDuration { get; set; }

			[JsonPropertyName("eval_count")]
			public int EvalCount { get; set; }

			[JsonPropertyName("eval_duration")]
			public long EvalDuration { get; set; }
		}

		public class ChatMessage
		{
            public string Role { get; set; }
            public string Content { get; set; }
        }
	}
}
