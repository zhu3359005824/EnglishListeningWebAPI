using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaEncoder.Domain
{
    public class MediaEncoderFactory
    {
        private readonly IEnumerable<IMediaEncoder> encoders ;

        public MediaEncoderFactory(IEnumerable<IMediaEncoder> encoders)
        {
            this.encoders = encoders;
        }

        public IMediaEncoder? Create(string outputType)
        {
            foreach (IMediaEncoder encoder in encoders)
            {
                if (encoder.Accept(outputType))
                {
                    return encoder;
                }



            }
            return null;
        }
    }
}
