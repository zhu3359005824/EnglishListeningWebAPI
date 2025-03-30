namespace MediaEncoder.Domain
{
    public interface IMediaEncoder
    {
        bool Accept(string OutputType);


        Task EncodeAsync(FileInfo sourceFile, FileInfo destinationFile, string destType, string[]? args, CancellationToken ct);
    }
}
