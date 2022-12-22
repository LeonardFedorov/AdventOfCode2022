[<AutoOpen>]
module Utilities

//F#'s native mod returns a residue with the same sign as the dividend, however for this we want to use the minimal non-negative CSR
let public realMod a b =
    let rawMod = a % b
    rawMod + if rawMod < LanguagePrimitives.GenericZero then b else LanguagePrimitives.GenericZero

let public realMod64 (a: int64) b =
    let rawMod = a % b
    rawMod + if rawMod < LanguagePrimitives.GenericZero then b else LanguagePrimitives.GenericZero

