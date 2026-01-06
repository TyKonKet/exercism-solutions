public static class TelemetryBuffer
{
    public static byte[] ToBuffer(long reading)
    {
        var buffer = new byte[9];
        var prefix = GetSizePrefix(reading);
        buffer[0] = prefix;
        var payload = BitConverter.GetBytes(reading);
        var payloadSize = GetPayloadSize(prefix);
        Array.Copy(payload, 0, buffer, 1, payloadSize);
        return buffer;
    }

    private static int GetPayloadSize(byte prefix) => prefix switch
    {
        2 => 2,
        254 => 2,
        4 => 4,
        252 => 4,
        248 => 8,
        _ => 0,
    };

    private static byte GetSizePrefix(long reading) => reading switch
    {
        >= 0 and <= 65_535 => 2,
        >= 65_536 and <= 2_147_483_647 => 252,
        >= 2_147_483_648 and <= 4_294_967_295 => 4,
        >= 4_294_967_296 and <= 9_223_372_036_854_775_807 => 248,
        < 0 and >= -32_768 => 254,
        < -32_768 and >= -2_147_483_648 => 252,
        < -2_147_483_648 and >= -9_223_372_036_854_775_808 => 248,
    };

    public static long FromBuffer(byte[] buffer)
    {
        if (buffer is null || buffer.Length == 0)
        {
            return 0;
        }

        return buffer[0] switch
        {
            2 when buffer.Length >= 3 => BitConverter.ToUInt16(buffer, 1),
            254 when buffer.Length >= 3 => BitConverter.ToInt16(buffer, 1),
            4 when buffer.Length >= 5 => BitConverter.ToUInt32(buffer, 1),
            252 when buffer.Length >= 5 => BitConverter.ToInt32(buffer, 1),
            248 when buffer.Length >= 9 => BitConverter.ToInt64(buffer, 1),
            _ => 0,
        };
    }
}
