using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Domain
{
    public interface IMediaEncoder
    {
        bool Accept(string OutputType);


        Task EncodeAsync(FileInfo sourceFile,FileInfo destinationFile, string[]? args,CancellationToken ct);
    }
}
