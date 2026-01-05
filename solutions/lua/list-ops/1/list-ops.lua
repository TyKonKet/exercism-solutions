-- This module provides functional programming operations for lists in Lua.

-- Appends two lists together, returning a new list containing all elements from xs followed by all elements from ys.
local function append(xs, ys)
  local result = {}
  for i = 1, #xs do
    table.insert(result, xs[i])
  end
  for i = 1, #ys do
    table.insert(result, ys[i])
  end
  return result
end

-- Concatenates multiple lists into a single list.
local function concat(...)
  local result = {}
  local sourceLists = { ... }
  for i = 1, #sourceLists do
    local currentList = sourceLists[i]
    for j = 1, #currentList do
      table.insert(result, currentList[j])
    end
  end
  return result
end

-- Returns the number of elements in the list xs.
local function length(xs)
  return #xs
end

-- Reverses the order of elements in the list xs.
local function reverse(xs)
  local result = {}
  for i = #xs, 1, -1 do
    table.insert(result, xs[i])
  end
  return result
end

-- Applies a function f to each element of the list xs from left to right, accumulating a result starting with the initial value.
local function foldl(xs, value, f)
  local acc = value
  for i = 1, #xs do
    acc = f(acc, xs[i])
  end
  return acc
end

-- Applies a function f to each element of the list xs from right to left, accumulating a result starting with the initial value.
local function foldr(xs, value, f)
  local acc = value
  for i = #xs, 1, -1 do
    acc = f(acc, xs[i])
  end
  return acc
end

-- Applies a function f to each element of the list xs and returns a new list with the results.
local function map(xs, f)
  local result = {}
  for i = 1, #xs do
    table.insert(result, f(xs[i]))
  end
  return result
end

-- Returns a new list containing only the elements from xs that satisfy the predicate function pred.
local function filter(xs, pred)
  local result = {}
  for i = 1, #xs do
    if pred(xs[i]) then
      table.insert(result, xs[i])
    end
  end
  return result
end

-- Exports the list operation functions for use in other modules.
return {
  append = append,
  concat = concat,
  length = length,
  reverse = reverse,
  map = map,
  foldl = foldl,
  foldr = foldr,
  filter = filter
}
