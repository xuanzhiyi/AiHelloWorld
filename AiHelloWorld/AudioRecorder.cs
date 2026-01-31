using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AiHelloWorld
{
    public class AudioRecorder : IDisposable
    {
        private WaveInEvent waveIn;
        private WaveFileWriter writer;

        private readonly string outputFile;

        public AudioRecorder(string outputFile)
        {
            this.outputFile = outputFile;
        }

		public void StartRecording()
        {
            waveIn = new WaveInEvent()
            {
                WaveFormat = new WaveFormat(16000, 16, 1)
            };

            writer = new WaveFileWriter(outputFile, waveIn.WaveFormat);

            waveIn.DataAvailable += (s, a) =>
            {
                writer.Write(a.Buffer, 0, a.BytesRecorded);
            };

            waveIn.RecordingStopped += (s, a) =>
			{
				waveIn?.Dispose();
				writer?.Dispose();
			};

            waveIn.StartRecording();
        }

        public void StopRecording()
        {
            waveIn?.StopRecording();
        }

        public void Dispose()
        {
            waveIn?.Dispose();
            writer?.Dispose();
		}
    }
}
