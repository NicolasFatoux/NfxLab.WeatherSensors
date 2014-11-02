#if NETWORK
using System;
using Microsoft.SPOT;
using System.Collections;
using System.IO;
using Microsoft.SPOT.Hardware;

namespace NfxLab.MicroFramework.Network
{
    public class PacketManager
    {
        static Random random = new Random();
        static byte currentId;
        static readonly byte[] StartSequence = new byte[3] { 0x66, 0x66, 0x66 };

        /// <summary>
        /// Builds a Secure Packet
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns></returns>
        public static byte[] BuildPacket(byte[] data)
        {
            if (data.Length > byte.MaxValue)
                throw new ArgumentException("data too big !", "data");

            int index;

            // Creating packet
            byte[] packet = new byte[StartSequence.Length + 4 + data.Length];

            // Writing start sequence
            Array.Copy(StartSequence, packet, StartSequence.Length);
            index = StartSequence.Length;

            // Writing header
            // - ID
            byte[] id = new byte[1];
            random.NextBytes(id);
            packet[index++] = id[0];

            // - Checksum
            packet[index++] = CheckSum(data);
            // - Length
            packet[index++] = (byte)data.Length;

            // Writing data
            Array.Copy(data, 0, packet, index, data.Length);

            return packet;
        }


        /// <summary>
        /// Reads Secure Packet data from a Stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>An IEnumerable of byte[] corresponding packet data</returns>
        public static IEnumerable Read(Stream stream)
        {
            while (stream.CanRead)
            {
                // Reading start sequence
                ReadStartSequence(stream);

                // Reading header
                // - ID
                byte id = (byte)stream.ReadByte();
                if (id == currentId)
                    // We skip the packet if it has already been read
                    continue;

                currentId = id;

                // - Checksum & size
                byte checksum = (byte)stream.ReadByte();
                byte size = (byte)stream.ReadByte();

                // Reading data
                byte[] data = new byte[size];
                for (int i = 0; i < size; i++)
                    data[i] = (byte)stream.ReadByte();

                // Integrity check
                byte dataChecksum = CheckSum(data);
                if (dataChecksum == checksum)
                    // Checksum OK
                    yield return data;
            }
        }

        /// <summary>
        /// Computes a checksum
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static byte CheckSum(byte[] data)
        {
            byte checksum = 0;

            foreach (byte d in data)
                checksum = (byte)(checksum ^ d);

            return checksum;
        }

        /// <summary>
        /// Reads a Secure Packet start sequence from a Stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        static void ReadStartSequence(Stream stream)
        {
            byte byte0 = (byte)stream.ReadByte();
            byte byte1 = (byte)stream.ReadByte();
            byte byte2 = (byte)stream.ReadByte();

            while (byte0 != StartSequence[0] || byte1 != StartSequence[1] || byte2 != StartSequence[2])
            {
                byte0 = byte1;
                byte1 = byte2;
                byte2 = (byte)stream.ReadByte();
            }
        }
    }
}
#endif