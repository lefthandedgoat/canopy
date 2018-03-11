namespace Canopy

open Canopy

/// Gives you:
/// - url operator: (!^)
/// - write operator: (<<)
/// - drag operator: (-->)
module Operators =
    (* documented/actions *)
    let ( !^ ) (u: string) =
        url u

    (* documented/actions *)
    let (<<) item text =
        writeC (context ()) text item

    (* documented/actions *)
    let (-->) cssSelectorA cssSelectorB =
        drag cssSelectorA cssSelectorB