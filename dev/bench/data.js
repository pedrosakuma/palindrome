window.BENCHMARK_DATA = {
  "lastUpdate": 1777338778225,
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
      },
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
          "id": "d279ad92261eddef2441c8cd922c0f72995062c2",
          "message": "feat: AVX-512 double-pumped checker (128 chars/iter)",
          "timestamp": "2026-04-28T00:34:27Z",
          "tree_id": "a2ea26bcb533abd8e5e75caa89489f0cb0b54b56",
          "url": "https://github.com/pedrosakuma/palindrome/commit/d279ad92261eddef2441c8cd922c0f72995062c2"
        },
        "date": 1777337635395,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 64)",
            "value": 767.8051382700602,
            "unit": "ns",
            "range": "± 0.9115983789196205"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 64)",
            "value": 71.18844735622406,
            "unit": "ns",
            "range": "± 0.3992281437605108"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 64)",
            "value": 112.39950529734294,
            "unit": "ns",
            "range": "± 0.39710182589755977"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 64)",
            "value": 98.42627080281575,
            "unit": "ns",
            "range": "± 0.17872919092063438"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 64)",
            "value": 101.40273563067119,
            "unit": "ns",
            "range": "± 0.08627158139001508"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 64)",
            "value": 97.8484833240509,
            "unit": "ns",
            "range": "± 0.3296775219275128"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 64)",
            "value": 99.8379457394282,
            "unit": "ns",
            "range": "± 0.07973266568134929"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 64)",
            "value": 15.329773038625717,
            "unit": "ns",
            "range": "± 0.042723401427981596"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 64)",
            "value": 9.719601904352507,
            "unit": "ns",
            "range": "± 0.024919046657799437"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 64)",
            "value": 64.63714921474457,
            "unit": "ns",
            "range": "± 0.48939298623963584"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 64)",
            "value": 76.36815281709035,
            "unit": "ns",
            "range": "± 0.10498907369774499"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 64)",
            "value": 765.8238660176595,
            "unit": "ns",
            "range": "± 2.230542387277783"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 64)",
            "value": 70.99398437830118,
            "unit": "ns",
            "range": "± 0.33592264626129514"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 64)",
            "value": 112.17802781860034,
            "unit": "ns",
            "range": "± 0.16616877151348314"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 64)",
            "value": 121.31611365538377,
            "unit": "ns",
            "range": "± 0.2093830436974354"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 64)",
            "value": 101.89098720749219,
            "unit": "ns",
            "range": "± 0.2422140664700581"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 64)",
            "value": 98.1623483300209,
            "unit": "ns",
            "range": "± 0.12538361832703837"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 64)",
            "value": 99.31617898207445,
            "unit": "ns",
            "range": "± 0.3032757012898349"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 64)",
            "value": 14.744754721011434,
            "unit": "ns",
            "range": "± 0.012698952546938331"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 64)",
            "value": 8.677411990364392,
            "unit": "ns",
            "range": "± 0.07396093130661024"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 64)",
            "value": 64.2086826654581,
            "unit": "ns",
            "range": "± 0.05289602869805914"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 64)",
            "value": 76.15168209870656,
            "unit": "ns",
            "range": "± 0.22691341610611457"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 1024)",
            "value": 7911.509877522786,
            "unit": "ns",
            "range": "± 23.981895441855485"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 1024)",
            "value": 1120.0633290608723,
            "unit": "ns",
            "range": "± 54.13248867381892"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 1024)",
            "value": 1480.8298606872559,
            "unit": "ns",
            "range": "± 0.25598009982009"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 1024)",
            "value": 1346.0222905476887,
            "unit": "ns",
            "range": "± 137.63909749991592"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 1024)",
            "value": 1329.027515411377,
            "unit": "ns",
            "range": "± 42.49753310744364"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 1024)",
            "value": 1378.2929058074951,
            "unit": "ns",
            "range": "± 121.8318149025996"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 1024)",
            "value": 1317.3642743428547,
            "unit": "ns",
            "range": "± 9.186571663042018"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 1024)",
            "value": 1002.0838902791342,
            "unit": "ns",
            "range": "± 0.3309971870261268"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 1024)",
            "value": 1082.6938400268555,
            "unit": "ns",
            "range": "± 0.26805166953970794"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 1024)",
            "value": 1144.917609532674,
            "unit": "ns",
            "range": "± 5.022515742400396"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 1024)",
            "value": 1169.217201868693,
            "unit": "ns",
            "range": "± 1.1069965064433"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 1024)",
            "value": 7982.096002197266,
            "unit": "ns",
            "range": "± 19.54563500924629"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 1024)",
            "value": 1098.2367909749348,
            "unit": "ns",
            "range": "± 15.256307776162927"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 1024)",
            "value": 1485.8267085735615,
            "unit": "ns",
            "range": "± 8.53152991815246"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 1024)",
            "value": 1259.9679097395676,
            "unit": "ns",
            "range": "± 2.1951096618405077"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 1024)",
            "value": 1328.3205473239605,
            "unit": "ns",
            "range": "± 35.95901639464823"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 1024)",
            "value": 1286.974159002304,
            "unit": "ns",
            "range": "± 32.57177386237991"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 1024)",
            "value": 1306.7046394348145,
            "unit": "ns",
            "range": "± 5.573227673406747"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 1024)",
            "value": 991.9333719106821,
            "unit": "ns",
            "range": "± 0.22314852757628686"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 1024)",
            "value": 1091.529545375279,
            "unit": "ns",
            "range": "± 0.2938277364410905"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 1024)",
            "value": 1159.878236134847,
            "unit": "ns",
            "range": "± 0.41028267081210495"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 1024)",
            "value": 1149.5061707814534,
            "unit": "ns",
            "range": "± 3.8911614731768585"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 16384)",
            "value": 139562.80143229166,
            "unit": "ns",
            "range": "± 132.3795529727893"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 16384)",
            "value": 42542.77561442057,
            "unit": "ns",
            "range": "± 100.26986931739603"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 16384)",
            "value": 47877.407165527344,
            "unit": "ns",
            "range": "± 1183.2915176438814"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 16384)",
            "value": 46282.44014485677,
            "unit": "ns",
            "range": "± 457.3107876193464"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 16384)",
            "value": 47074.24645996094,
            "unit": "ns",
            "range": "± 273.65189487033706"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 16384)",
            "value": 46477.425618489586,
            "unit": "ns",
            "range": "± 154.10993130752718"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 16384)",
            "value": 49336.91971842448,
            "unit": "ns",
            "range": "± 2588.2635871982625"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 16384)",
            "value": 14528.367914835611,
            "unit": "ns",
            "range": "± 1.6035568370045636"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 16384)",
            "value": 15543.176767985025,
            "unit": "ns",
            "range": "± 1.9564801926328172"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 16384)",
            "value": 17023.934661865234,
            "unit": "ns",
            "range": "± 1.0899284549494281"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 16384)",
            "value": 18029.611389160156,
            "unit": "ns",
            "range": "± 13.534436623872967"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 16384)",
            "value": 145248.4715657552,
            "unit": "ns",
            "range": "± 288.82262104065194"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 16384)",
            "value": 42077.127990722656,
            "unit": "ns",
            "range": "± 127.85963844168012"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 16384)",
            "value": 48007.285879952564,
            "unit": "ns",
            "range": "± 210.29834556527476"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 16384)",
            "value": 46669.91933969351,
            "unit": "ns",
            "range": "± 633.614340531026"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 16384)",
            "value": 47374.07734781901,
            "unit": "ns",
            "range": "± 817.0244392090866"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 16384)",
            "value": 47279.978969573975,
            "unit": "ns",
            "range": "± 596.8730418128324"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 16384)",
            "value": 47855.05542399089,
            "unit": "ns",
            "range": "± 618.2775026356398"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 16384)",
            "value": 28721.18264567057,
            "unit": "ns",
            "range": "± 6.31821989156347"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 16384)",
            "value": 15580.902197265625,
            "unit": "ns",
            "range": "± 3.573121955514507"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 16384)",
            "value": 17019.58322797503,
            "unit": "ns",
            "range": "± 3.681635523007746"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 16384)",
            "value": 18141.22262776693,
            "unit": "ns",
            "range": "± 14.602912796443862"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 262144)",
            "value": 2540358.7721354165,
            "unit": "ns",
            "range": "± 6559.039136646634"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 262144)",
            "value": 903264.0221354166,
            "unit": "ns",
            "range": "± 327.3867064780574"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 262144)",
            "value": 1103085.158203125,
            "unit": "ns",
            "range": "± 1707.6039367933772"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 262144)",
            "value": 1087902.0533854167,
            "unit": "ns",
            "range": "± 144.86456552647425"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 262144)",
            "value": 1090013.068359375,
            "unit": "ns",
            "range": "± 3169.029356200276"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 262144)",
            "value": 1090345.0078125,
            "unit": "ns",
            "range": "± 1601.3198103910754"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 262144)",
            "value": 1114466.7565104167,
            "unit": "ns",
            "range": "± 1588.8343744755064"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 262144)",
            "value": 259840.8816731771,
            "unit": "ns",
            "range": "± 95.43013102110476"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 262144)",
            "value": 261106.5050455729,
            "unit": "ns",
            "range": "± 32.421756041281746"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 262144)",
            "value": 282863.67985026044,
            "unit": "ns",
            "range": "± 79.91804899055079"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 262144)",
            "value": 286914.9482421875,
            "unit": "ns",
            "range": "± 28.620694981979838"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 262144)",
            "value": 2554480.1875,
            "unit": "ns",
            "range": "± 11524.199434868431"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 262144)",
            "value": 901565.5908854167,
            "unit": "ns",
            "range": "± 1091.649591934717"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 262144)",
            "value": 1105659.6107271635,
            "unit": "ns",
            "range": "± 4261.0840763855895"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 262144)",
            "value": 1094563.2746930805,
            "unit": "ns",
            "range": "± 7791.365108545796"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 262144)",
            "value": 1091221.8208383413,
            "unit": "ns",
            "range": "± 4189.732404676593"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 262144)",
            "value": 1092354.546048678,
            "unit": "ns",
            "range": "± 2696.8213827133645"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 262144)",
            "value": 1117037.7025240385,
            "unit": "ns",
            "range": "± 4965.2242911818075"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 262144)",
            "value": 260627.3763671875,
            "unit": "ns",
            "range": "± 59.085257430756485"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 262144)",
            "value": 262015.01105608259,
            "unit": "ns",
            "range": "± 81.90602285443194"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 262144)",
            "value": 282149.3419363839,
            "unit": "ns",
            "range": "± 46.813729900470626"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 262144)",
            "value": 293786.2836538461,
            "unit": "ns",
            "range": "± 138.0088605752167"
          }
        ]
      }
    ],
    "Palindrome (Azure Dsv5 Ice Lake)": [
      {
        "commit": {
          "author": {
            "name": "Pedro Sakuma Travi",
            "username": "pedrosakuma",
            "email": "pedrosakuma@users.noreply.github.com"
          },
          "committer": {
            "name": "Pedro Sakuma Travi",
            "username": "pedrosakuma",
            "email": "pedrosakuma@users.noreply.github.com"
          },
          "id": "d279ad92261eddef2441c8cd922c0f72995062c2",
          "message": "feat: AVX-512 double-pumped checker (128 chars/iter)",
          "timestamp": "2026-04-28T00:34:27Z",
          "url": "https://github.com/pedrosakuma/palindrome/commit/d279ad92261eddef2441c8cd922c0f72995062c2"
        },
        "date": 1777338777938,
        "tool": "benchmarkdotnet",
        "benches": [
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 64)",
            "value": 750.3431905110677,
            "unit": "ns",
            "range": "± 4.796382764495345"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 64)",
            "value": 63.3001358906428,
            "unit": "ns",
            "range": "± 2.8584875294671117"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 64)",
            "value": 99.30260475476582,
            "unit": "ns",
            "range": "± 0.11424767414094665"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 64)",
            "value": 92.53375299771626,
            "unit": "ns",
            "range": "± 1.88411612899729"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 64)",
            "value": 91.08608043193817,
            "unit": "ns",
            "range": "± 1.047776468265365"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 64)",
            "value": 88.5574670235316,
            "unit": "ns",
            "range": "± 1.8917324965949034"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 64)",
            "value": 88.85604580243428,
            "unit": "ns",
            "range": "± 1.243292013499008"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 64)",
            "value": 11.868531182408333,
            "unit": "ns",
            "range": "± 0.05808244485858999"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 64)",
            "value": 5.386434182524681,
            "unit": "ns",
            "range": "± 0.04974960940475256"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 64)",
            "value": 53.26828461885452,
            "unit": "ns",
            "range": "± 0.45441237429015097"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 64)",
            "value": 66.79350674152374,
            "unit": "ns",
            "range": "± 3.8546304901448623"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 64)",
            "value": 763.5669623057048,
            "unit": "ns",
            "range": "± 3.4170171741076603"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 64)",
            "value": 52.904467821121216,
            "unit": "ns",
            "range": "± 0.39997515597285666"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 64)",
            "value": 106.75783121585846,
            "unit": "ns",
            "range": "± 1.873274163653158"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 64)",
            "value": 93.5008900086085,
            "unit": "ns",
            "range": "± 1.5158063657831933"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 64)",
            "value": 94.22157264550528,
            "unit": "ns",
            "range": "± 1.6559646311375393"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 64)",
            "value": 92.16804489294688,
            "unit": "ns",
            "range": "± 1.1067907064232023"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 64)",
            "value": 90.90230627854665,
            "unit": "ns",
            "range": "± 1.1138203237462727"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 64)",
            "value": 11.538924908638,
            "unit": "ns",
            "range": "± 0.04596401412816419"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 64)",
            "value": 5.6862152751003,
            "unit": "ns",
            "range": "± 0.04111088945858968"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 64)",
            "value": 63.096243500709534,
            "unit": "ns",
            "range": "± 2.2030310072600923"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 64)",
            "value": 67.07396891483894,
            "unit": "ns",
            "range": "± 1.8935630449308394"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 1024)",
            "value": 6959.481168111165,
            "unit": "ns",
            "range": "± 168.79323323233262"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 1024)",
            "value": 997.8853804270426,
            "unit": "ns",
            "range": "± 30.74489441317785"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 1024)",
            "value": 1370.3112030029297,
            "unit": "ns",
            "range": "± 12.859796098509898"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 1024)",
            "value": 1165.0457782745361,
            "unit": "ns",
            "range": "± 45.49217776986462"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 1024)",
            "value": 1180.9441165924072,
            "unit": "ns",
            "range": "± 33.995205304363836"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 1024)",
            "value": 1190.3284759521484,
            "unit": "ns",
            "range": "± 24.55635085221133"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 1024)",
            "value": 1210.004976908366,
            "unit": "ns",
            "range": "± 64.17729050357296"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 1024)",
            "value": 858.45796362559,
            "unit": "ns",
            "range": "± 1.705278975240237"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 1024)",
            "value": 972.5598398844401,
            "unit": "ns",
            "range": "± 4.314912967148219"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 1024)",
            "value": 974.374568939209,
            "unit": "ns",
            "range": "± 0.8609153097205725"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 1024)",
            "value": 1014.9958063761393,
            "unit": "ns",
            "range": "± 7.8155752277751125"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 1024)",
            "value": 6674.625612531389,
            "unit": "ns",
            "range": "± 33.242220959278335"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 1024)",
            "value": 930.7108046213785,
            "unit": "ns",
            "range": "± 10.15107396781296"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 1024)",
            "value": 1355.7971002679121,
            "unit": "ns",
            "range": "± 28.552211291621628"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 1024)",
            "value": 1135.2573640099888,
            "unit": "ns",
            "range": "± 32.81188312132141"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 1024)",
            "value": 1153.6182869997892,
            "unit": "ns",
            "range": "± 28.178941023055497"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 1024)",
            "value": 1134.7765947977703,
            "unit": "ns",
            "range": "± 28.74686941018099"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 1024)",
            "value": 1110.7265126546224,
            "unit": "ns",
            "range": "± 32.95381358306331"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 1024)",
            "value": 861.2247993605478,
            "unit": "ns",
            "range": "± 1.3561473616533968"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 1024)",
            "value": 973.1137578146798,
            "unit": "ns",
            "range": "± 1.7574821891858279"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 1024)",
            "value": 1002.07930615743,
            "unit": "ns",
            "range": "± 2.812518808811729"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 1024)",
            "value": 1016.1033055623373,
            "unit": "ns",
            "range": "± 5.633222498790607"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 16384)",
            "value": 126618.87292480469,
            "unit": "ns",
            "range": "± 5187.474006704051"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 16384)",
            "value": 37915.338928222656,
            "unit": "ns",
            "range": "± 397.99980166417066"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 16384)",
            "value": 42162.70166015625,
            "unit": "ns",
            "range": "± 3740.325694104711"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 16384)",
            "value": 38355.77734375,
            "unit": "ns",
            "range": "± 273.391659146977"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 16384)",
            "value": 39410.35341389974,
            "unit": "ns",
            "range": "± 850.3358548615167"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 16384)",
            "value": 38007.829264322914,
            "unit": "ns",
            "range": "± 956.7784589123798"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 16384)",
            "value": 36797.62458292643,
            "unit": "ns",
            "range": "± 152.29531350853014"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 16384)",
            "value": 12892.821461995443,
            "unit": "ns",
            "range": "± 98.77530579100592"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 16384)",
            "value": 14190.349505106607,
            "unit": "ns",
            "range": "± 86.92161978736837"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 16384)",
            "value": 14715.41781616211,
            "unit": "ns",
            "range": "± 19.2923383568204"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 16384)",
            "value": 15996.127090454102,
            "unit": "ns",
            "range": "± 61.478841685624985"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 16384)",
            "value": 123396.95933314732,
            "unit": "ns",
            "range": "± 560.9885289997095"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 16384)",
            "value": 37027.15200195312,
            "unit": "ns",
            "range": "± 247.15484553325024"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 16384)",
            "value": 40491.0300394694,
            "unit": "ns",
            "range": "± 486.48614641826066"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 16384)",
            "value": 36504.939408365884,
            "unit": "ns",
            "range": "± 403.0743709367721"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 16384)",
            "value": 39072.62086406507,
            "unit": "ns",
            "range": "± 849.4101211121869"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 16384)",
            "value": 39309.016829427084,
            "unit": "ns",
            "range": "± 725.9701411300967"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 16384)",
            "value": 37099.808340890064,
            "unit": "ns",
            "range": "± 397.1400912971947"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 16384)",
            "value": 14078.427241118057,
            "unit": "ns",
            "range": "± 539.9541003455091"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 16384)",
            "value": 15070.433414713541,
            "unit": "ns",
            "range": "± 144.6643837243744"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 16384)",
            "value": 15792.421471228967,
            "unit": "ns",
            "range": "± 175.57947425471315"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 16384)",
            "value": 17401.015211995444,
            "unit": "ns",
            "range": "± 287.3544677260421"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 262144)",
            "value": 2488219.2897135415,
            "unit": "ns",
            "range": "± 10970.152441722954"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 262144)",
            "value": 957136.2457682291,
            "unit": "ns",
            "range": "± 17473.398853644012"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 262144)",
            "value": 1125782.2177734375,
            "unit": "ns",
            "range": "± 53719.22289466209"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 262144)",
            "value": 1025244.0992838541,
            "unit": "ns",
            "range": "± 51860.69115699053"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 262144)",
            "value": 980459.498046875,
            "unit": "ns",
            "range": "± 12778.536803286917"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 262144)",
            "value": 983229.1168619791,
            "unit": "ns",
            "range": "± 13728.568261352284"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 262144)",
            "value": 1031021.158203125,
            "unit": "ns",
            "range": "± 73635.22241591317"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 262144)",
            "value": 238683.4816080729,
            "unit": "ns",
            "range": "± 1403.4194369298764"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 262144)",
            "value": 239941.48388671875,
            "unit": "ns",
            "range": "± 1918.0778672975694"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 262144)",
            "value": 247229.89111328125,
            "unit": "ns",
            "range": "± 1433.8333303438217"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 262144)",
            "value": 270030.8948567708,
            "unit": "ns",
            "range": "± 2541.3480016156204"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Naive(Length: 262144)",
            "value": 2442298.001953125,
            "unit": "ns",
            "range": "± 33621.098712480234"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointer(Length: 262144)",
            "value": 914276.6351036659,
            "unit": "ns",
            "range": "± 9559.914329175535"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.NormalizedTwoPointer(Length: 262144)",
            "value": 1067522.2328125,
            "unit": "ns",
            "range": "± 16207.051007046914"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.VectorT(Length: 262144)",
            "value": 1008519.8683035715,
            "unit": "ns",
            "range": "± 14540.349723605508"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector128(Length: 262144)",
            "value": 1011821.7983072917,
            "unit": "ns",
            "range": "± 10053.163681673172"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector256(Length: 262144)",
            "value": 1004095.3533203125,
            "unit": "ns",
            "range": "± 13602.03344258168"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.Vector512(Length: 262144)",
            "value": 1013459.3631510417,
            "unit": "ns",
            "range": "± 12587.027378568588"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimd(Length: 262144)",
            "value": 235410.1668701172,
            "unit": "ns",
            "range": "± 3669.31989061184"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdByte(Length: 262144)",
            "value": 243455.642124721,
            "unit": "ns",
            "range": "± 1103.3704890796475"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512(Length: 262144)",
            "value": 255860.41998697916,
            "unit": "ns",
            "range": "± 2949.637781495333"
          },
          {
            "name": "Palindrome.Benchmarks.PalindromeBenchmarks.TwoPointerSimdAvx512x2(Length: 262144)",
            "value": 276611.97430889425,
            "unit": "ns",
            "range": "± 1356.0216337604513"
          }
        ]
      }
    ]
  }
}