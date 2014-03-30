module canopy.levenshtein

type result = { selector : string; distance : int }

// [snippet:Remove first ocurrence from list]
let rec remove char lst =
    match lst with
    | h::t when h = char -> t
    | h::t -> h::remove char t
    | _ -> []

//taken from http://fssnip.net/raw/bj
let levenshtein word1 word2 =
    let preprocess = fun (str : string) -> str.ToLower().ToCharArray()
    let chars1, chars2 = preprocess word1, preprocess word2
    let m, n = chars1.Length, chars2.Length
    let table : int[,] = Array2D.zeroCreate (m + 1) (n + 1)
    for i in 0..m do
        for j in 0..n do
            match i, j with
            | i, 0 -> table.[i, j] <- i
            | 0, j -> table.[i, j] <- j
            | _, _ ->
                let delete = table.[i-1, j] + 1
                let insert = table.[i, j-1] + 1
                //cost of substitution is 2
                let substitute = 
                    if chars1.[i - 1] = chars2.[j - 1] 
                        then table.[i-1, j-1] //same character
                        else table.[i-1, j-1] + 2
                table.[i, j] <- List.min [delete; insert; substitute]
    { selector = word2; distance = table.[m, n] }