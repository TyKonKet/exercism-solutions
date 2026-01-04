return function(n)
  result = ""
  divisible = false
  if (n % 3) == 0 then
    result = result .. "Pling"
    divisible = true
  end
  if (n % 5) == 0 then
    result = result .. "Plang"
    divisible = true
  end
  if (n % 7) == 0 then
    result = result .. "Plong"
    divisible = true
  end
  if (divisible) then
    return result
  else
    return tostring(n)
  end
end
