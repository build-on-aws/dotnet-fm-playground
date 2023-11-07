using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetFMPlayground.Core.Models.ModelResponse
{
    public class AI21LabsJurassic2Response : IFoundationModelResponse
    {
        //{"id":1234,"prompt":{"text":"Hello Jurassic","tokens":[{"generatedToken":{"token":"▁Hello","logprob":-8.374051094055176,"raw_logprob":-8.374051094055176},"topTokens":null,"textRange":{"start":0,"end":5}},{ "generatedToken":{ "token":"▁","logprob":-5.375925064086914,"raw_logprob":-5.375925064086914},"topTokens":null,"textRange":{ "start":5,"end":6} },{ "generatedToken":{ "token":"Jurassic","logprob":-12.566287994384766,"raw_logprob":-12.566287994384766},"topTokens":null,"textRange":{ "start":6,"end":14} }]},"completions":[{"data":{"text":"\nHello, welcome to Jurassic! What would you like to know or discuss about jurassi","tokens":[{"generatedToken":{"token":"<|newline|>","logprob":-0.2771032452583313,"raw_logprob":-0.2771032452583313},"topTokens":null,"textRange":{ "start":0,"end":1}},{ "generatedToken":{ "token":"▁Hello","logprob":-1.1910626888275146,"raw_logprob":-1.1910626888275146},"topTokens":null,"textRange":{ "start":1,"end":6} },{ "generatedToken":{ "token":",","logprob":-1.5938622951507568,"raw_logprob":-1.5938622951507568},"topTokens":null,"textRange":{ "start":6,"end":7} },{ "generatedToken":{ "token":"▁welcome▁to","logprob":-5.078219413757324,"raw_logprob":-5.078219413757324},"topTokens":null,"textRange":{ "start":7,"end":18} },{ "generatedToken":{ "token":"▁","logprob":-1.185720443725586,"raw_logprob":-1.185720443725586},"topTokens":null,"textRange":{ "start":18,"end":19} },{ "generatedToken":{ "token":"Jurassic","logprob":-0.011890722438693047,"raw_logprob":-0.011890722438693047},"topTokens":null,"textRange":{ "start":19,"end":27} },{ "generatedToken":{ "token":"!","logprob":-0.3744388818740845,"raw_logprob":-0.3744388818740845},"topTokens":null,"textRange":{ "start":27,"end":28} },{ "generatedToken":{ "token":"▁What","logprob":-5.624349594116211,"raw_logprob":-5.624349594116211},"topTokens":null,"textRange":{ "start":28,"end":33} },{ "generatedToken":{ "token":"▁would▁you▁like▁to","logprob":-0.4887925386428833,"raw_logprob":-0.4887925386428833},"topTokens":null,"textRange":{ "start":33,"end":51} },{ "generatedToken":{ "token":"▁know","logprob":-0.03536876663565636,"raw_logprob":-0.03536876663565636},"topTokens":null,"textRange":{ "start":51,"end":56} },{ "generatedToken":{ "token":"▁or","logprob":-0.018549775704741478,"raw_logprob":-0.018549775704741478},"topTokens":null,"textRange":{ "start":56,"end":59} },{ "generatedToken":{ "token":"▁discuss","logprob":-0.21317188441753387,"raw_logprob":-0.21317188441753387},"topTokens":null,"textRange":{ "start":59,"end":67} },{ "generatedToken":{ "token":"▁about","logprob":-1.4178533554077148,"raw_logprob":-1.4178533554077148},"topTokens":null,"textRange":{ "start":67,"end":73} },{ "generatedToken":{ "token":"▁","logprob":-2.306314706802368,"raw_logprob":-2.306314706802368},"topTokens":null,"textRange":{ "start":73,"end":74} },{ "generatedToken":{ "token":"jur","logprob":-4.701274871826172,"raw_logprob":-4.701274871826172},"topTokens":null,"textRange":{ "start":74,"end":77} },{ "generatedToken":{ "token":"assi","logprob":-0.0061679016798734665,"raw_logprob":-0.0061679016798734665},"topTokens":null,"textRange":{ "start":77,"end":81} }]},"finishReason":{ "reason":"length","length":16}}]}
        public uint? Id { get; init; }

        public Jurassic2Prompt? Prompt { get; init; }

        public class Jurassic2TokenDetails
        {
            public string? Token { get; init; }

            public decimal? LogProb { get; init; }

            public decimal? Raw_LogProb { get; init; }
        }

        public class Jurassic2TokenRange
        {
            public int? Start { get; init; }

            public int? End { get; init; }
        }

        public class Jurassic2Token
        {
            public Jurassic2TokenDetails? GeneratedToken { get; init; }

            public Jurassic2TokenRange? TextRange { get; init; }

        }

        public class Jurassic2Prompt
        {
            public string? Text { get; init; }

            public IEnumerable<Jurassic2Token>? Tokens { get; init; }
        }

        public class Jurassic2CompletionData
        {
            public string? Text { get; init; }

            public IEnumerable<Jurassic2Token>? Tokens { get; init; }
        }

        public class Jurassic2FinishReason
        {
            public string? Reason { get; init; }

            public uint? Length { get; init; }

        }

        public class Jurassic2Completion
        {
            public Jurassic2CompletionData? Data { get; init; }

            public Jurassic2FinishReason? FinishReason { get; init; }
        }

        public IEnumerable<Jurassic2Completion>? Completions { get; init; }


        public string? GetResponse()
        {
            return Completions?.FirstOrDefault()?.Data?.Text;
        }

        public string? GetStopReason()
        {
            return Completions?.FirstOrDefault()?.FinishReason?.Reason;
        }
    }
}