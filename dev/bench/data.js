window.BENCHMARK_DATA = {
  "lastUpdate": 1777335471371,
  "repoUrl": "https://github.com/pedrosakuma/palindrome",
  "entries": {
    "Benchmark": [
      {
        "commit": {
          "author": {
            "email": "pedrosakuma@users.noreply.github.com",
            "name": "Pedro Sakuma Travi",
            "username": "pedrosakuma"
          },
          "committer": {
            "email": "pedrosakuma@users.noreply.github.com",
            "name": "Pedro Sakuma Travi",
            "username": "pedrosakuma"
          },
          "distinct": true,
          "id": "5d78a4874ffe745f5e3fbdab481979d82add75ad",
          "message": "ci: fix BDN report path (full-compressed instead of full-compact)",
          "timestamp": "2026-04-27T23:59:13Z",
          "tree_id": "47feb021fe6039b52b0de84706b89eb5e34b9b67",
          "url": "https://github.com/pedrosakuma/palindrome/commit/5d78a4874ffe745f5e3fbdab481979d82add75ad"
        },
        "date": 1777335470758,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 64)",
            "value": 561.2724695205688,
            "unit": "ns",
            "range": "± 19.35560990159935"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 64)",
            "value": 56.605105221271515,
            "unit": "ns",
            "range": "± 2.536609081657802"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 64)",
            "value": 92.1662939786911,
            "unit": "ns",
            "range": "± 0.08150945486667183"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 64)",
            "value": 80.06982596715291,
            "unit": "ns",
            "range": "± 0.6724746575708085"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 64)",
            "value": 81.58708695570628,
            "unit": "ns",
            "range": "± 0.10173220486053433"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 64)",
            "value": 79.49980453650157,
            "unit": "ns",
            "range": "± 0.18455279090004104"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 64)",
            "value": 79.6572140455246,
            "unit": "ns",
            "range": "± 0.05048918930386748"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 64)",
            "value": 15.875358174244562,
            "unit": "ns",
            "range": "± 0.009009583307218608"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 64)",
            "value": 7.470059101780255,
            "unit": "ns",
            "range": "± 0.02516558101200213"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 64)",
            "value": 54.65113842487335,
            "unit": "ns",
            "range": "± 0.027353825031860124"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 64)",
            "value": 531.2897259848459,
            "unit": "ns",
            "range": "± 0.8487343531567108"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 64)",
            "value": 54.0171160697937,
            "unit": "ns",
            "range": "± 0.05849534027590242"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 64)",
            "value": 91.864086829699,
            "unit": "ns",
            "range": "± 0.04128738689777548"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 64)",
            "value": 79.02327608068784,
            "unit": "ns",
            "range": "± 0.07537297745924515"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 64)",
            "value": 81.74059301156264,
            "unit": "ns",
            "range": "± 0.06834830595578084"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 64)",
            "value": 79.35115159409386,
            "unit": "ns",
            "range": "± 0.09123509173479484"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 64)",
            "value": 79.25079298936404,
            "unit": "ns",
            "range": "± 0.05649926224380801"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 64)",
            "value": 14.244075457255045,
            "unit": "ns",
            "range": "± 0.015277918153720238"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 64)",
            "value": 7.150951246802624,
            "unit": "ns",
            "range": "± 0.013079503624669333"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 64)",
            "value": 57.428949773311615,
            "unit": "ns",
            "range": "± 0.1624477162995295"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 1024)",
            "value": 5674.365954081218,
            "unit": "ns",
            "range": "± 65.04760661096294"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 1024)",
            "value": 917.0280303955078,
            "unit": "ns",
            "range": "± 5.359112161861666"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 1024)",
            "value": 1242.070515314738,
            "unit": "ns",
            "range": "± 6.499238901199442"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 1024)",
            "value": 1135.2207660675049,
            "unit": "ns",
            "range": "± 2.0500883104343353"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 1024)",
            "value": 1163.5917650858562,
            "unit": "ns",
            "range": "± 2.9475394925877403"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 1024)",
            "value": 1145.0235201517742,
            "unit": "ns",
            "range": "± 3.9943456765932703"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 1024)",
            "value": 1263.281836827596,
            "unit": "ns",
            "range": "± 0.7539723058992063"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 1024)",
            "value": 878.9899806976318,
            "unit": "ns",
            "range": "± 0.13461452565316595"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 1024)",
            "value": 909.2096455891927,
            "unit": "ns",
            "range": "± 0.549170506909034"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 1024)",
            "value": 1331.4421367645264,
            "unit": "ns",
            "range": "± 5.799149002140504"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 1024)",
            "value": 5643.177966308594,
            "unit": "ns",
            "range": "± 37.26338213415523"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 1024)",
            "value": 917.244647539579,
            "unit": "ns",
            "range": "± 8.637940097944266"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 1024)",
            "value": 1237.8957199369158,
            "unit": "ns",
            "range": "± 1.2776044680239418"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 1024)",
            "value": 1147.2120718638103,
            "unit": "ns",
            "range": "± 2.269245977475447"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 1024)",
            "value": 1178.1945828755697,
            "unit": "ns",
            "range": "± 2.1946439479112674"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 1024)",
            "value": 1640.1308901468913,
            "unit": "ns",
            "range": "± 3.242474573372779"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 1024)",
            "value": 1131.055352483477,
            "unit": "ns",
            "range": "± 1.801676767432509"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 1024)",
            "value": 905.6657700856526,
            "unit": "ns",
            "range": "± 0.07462618628667929"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 1024)",
            "value": 908.7176366170247,
            "unit": "ns",
            "range": "± 1.0385364552994039"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 1024)",
            "value": 1107.2972286224365,
            "unit": "ns",
            "range": "± 2.439822517278309"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 16384)",
            "value": 85148.462890625,
            "unit": "ns",
            "range": "± 308.49387253529494"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 16384)",
            "value": 15771.567647298178,
            "unit": "ns",
            "range": "± 240.98553791674888"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 16384)",
            "value": 20936.147867838543,
            "unit": "ns",
            "range": "± 112.1307040027163"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 16384)",
            "value": 18964.094635009766,
            "unit": "ns",
            "range": "± 16.359880860499548"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 16384)",
            "value": 19456.687093098957,
            "unit": "ns",
            "range": "± 27.122759794133735"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 16384)",
            "value": 19109.31601969401,
            "unit": "ns",
            "range": "± 183.94346134786386"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 16384)",
            "value": 18897.20502726237,
            "unit": "ns",
            "range": "± 31.128934619768533"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 16384)",
            "value": 13490.824427286783,
            "unit": "ns",
            "range": "± 21.45695430114636"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 16384)",
            "value": 13224.477879842123,
            "unit": "ns",
            "range": "± 1.8428044075436383"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 16384)",
            "value": 16579.067576090496,
            "unit": "ns",
            "range": "± 7.4339704349412115"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 16384)",
            "value": 87390.14741734097,
            "unit": "ns",
            "range": "± 639.0232749238833"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 16384)",
            "value": 15804.57218017578,
            "unit": "ns",
            "range": "± 98.69155057489532"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 16384)",
            "value": 21110.599420166014,
            "unit": "ns",
            "range": "± 374.98482823743336"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 16384)",
            "value": 19001.075710042318,
            "unit": "ns",
            "range": "± 20.960092162696313"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 16384)",
            "value": 19564.023839314777,
            "unit": "ns",
            "range": "± 22.447845065752134"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 16384)",
            "value": 18975.43815714518,
            "unit": "ns",
            "range": "± 15.306328096325835"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 16384)",
            "value": 21480.90018572126,
            "unit": "ns",
            "range": "± 18.038492666840526"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 16384)",
            "value": 12866.083194986979,
            "unit": "ns",
            "range": "± 1.8345769648756396"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 16384)",
            "value": 24992.23935953776,
            "unit": "ns",
            "range": "± 28.383325203544608"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 16384)",
            "value": 20055.394963191105,
            "unit": "ns",
            "range": "± 18.012856426368337"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 262144)",
            "value": 2116085.3893229165,
            "unit": "ns",
            "range": "± 2509.2387494457703"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 262144)",
            "value": 727045.8020833334,
            "unit": "ns",
            "range": "± 6163.979750249183"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 262144)",
            "value": 915544.634765625,
            "unit": "ns",
            "range": "± 4827.573933994528"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 262144)",
            "value": 846628.8914388021,
            "unit": "ns",
            "range": "± 406.0955578628619"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 262144)",
            "value": 856622.7835286459,
            "unit": "ns",
            "range": "± 3506.641392982821"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 262144)",
            "value": 851706.3110351562,
            "unit": "ns",
            "range": "± 5236.453835475465"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 262144)",
            "value": 848739.1354166666,
            "unit": "ns",
            "range": "± 2499.5025945579273"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 262144)",
            "value": 208714.1513671875,
            "unit": "ns",
            "range": "± 22.325565344913212"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 262144)",
            "value": 207407.91080729166,
            "unit": "ns",
            "range": "± 41.54666498333646"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 262144)",
            "value": 269899.97867838544,
            "unit": "ns",
            "range": "± 143.00484558740052"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 262144)",
            "value": 2145005.5341145834,
            "unit": "ns",
            "range": "± 29238.1539246892"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 262144)",
            "value": 729206.5352213542,
            "unit": "ns",
            "range": "± 8204.23557832741"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 262144)",
            "value": 913206.1352213542,
            "unit": "ns",
            "range": "± 7466.966015216502"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 262144)",
            "value": 840959.8565204327,
            "unit": "ns",
            "range": "± 374.9540520190839"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 262144)",
            "value": 848542.8974609375,
            "unit": "ns",
            "range": "± 509.3069020103039"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 262144)",
            "value": 839117.2362905649,
            "unit": "ns",
            "range": "± 296.48980591062906"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 262144)",
            "value": 880671.1576021635,
            "unit": "ns",
            "range": "± 353.2477934835557"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 262144)",
            "value": 209029.6109095982,
            "unit": "ns",
            "range": "± 32.05845816650011"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 262144)",
            "value": 213335.3109788161,
            "unit": "ns",
            "range": "± 15.52131127569535"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 262144)",
            "value": 332157.06295572914,
            "unit": "ns",
            "range": "± 532.0761093464315"
          }
        ]
      }
    ]
  }
}