using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Domain
{
    public interface IMediaEncoderRepository
    {

        Task<EncodingItem?> FindOneFinishAsync(string fileSHA256Hash,long fileByteSize);


        Task<EncodingItem[]> FindAsync(ItemStatus itemStatus);
    }
}
