return function()
    -- Internal state shared by every operation on this list.
    local list = {
        head = nil,
        tail = nil,
        size = 0,
    }

    -- Node factory keeps the payload consistent for every doubly linked node.
    local function createNode(value)
        return {
            value = value,
            next = nil,
            prev = nil,
        }
    end

    -- Removes a node from the list without returning it.
    local function detach(node)
        -- Step 1: bridge the previous node (if any) forward to skip this node
        if node.prev ~= nil then
            node.prev.next = node.next
        else
            list.head = node.next
        end

        -- Step 2: bridge the next node (if any) backward to skip this node
        if node.next ~= nil then
            node.next.prev = node.prev
        else
            list.tail = node.prev
        end

        -- Step 3: decrement size to reflect that a node was removed
        list.size = list.size - 1
        -- Step 4: clear pointers to help with garbage collection and avoid accidental reuse
        node.next = nil
        node.prev = nil
    end

    -- Links a node as the new tail and increments the size counter.
    local function attachTail(node)
        -- Step 1: link the new node to the previous tail
        node.prev = list.tail
        node.next = nil

        -- Step 2: plug the previous tail forward, or initialize the head if the list was empty
        if list.tail ~= nil then
            list.tail.next = node
        else
            list.head = node
        end

        -- Step 3: reassign the tail reference and bump the size
        list.tail = node
        list.size = list.size + 1
    end

    -- Links a node as the new head and increments the size counter.
    local function attachHead(node)
        -- Step 1: link the new node to the previous head
        node.next = list.head
        node.prev = nil

        -- Step 2: plug the previous head back, or initialize the tail if the list was empty
        if list.head ~= nil then
            list.head.prev = node
        else
            list.tail = node
        end

        -- Step 3: reassign the head reference and bump the size
        list.head = node
        list.size = list.size + 1
    end

    -- Appends a value to the end of the list.
    local function push(_, value)
        attachTail(createNode(value))
    end

    -- Removes and returns the value at the end of the list.
    local function pop()
        local tail = list.tail

        if tail == nil then
            return nil
        end

        detach(tail)
        return tail.value
    end

    -- Removes and returns the value at the start of the list.
    local function shift()
        local head = list.head

        if head == nil then
            return nil
        end

        detach(head)
        return head.value
    end

    -- Prepends a value to the start of the list.
    local function unshift(_, value)
        attachHead(createNode(value))
    end

    -- Returns the number of elements currently stored in the list.
    local function count()
        return list.size
    end

    -- Walks the list and removes the first node matching the provided value.
    local function delete(_, value)
        local current = list.head

        while current ~= nil do
            if current.value == value then
                detach(current)
                return
            end
            current = current.next
        end
    end

    -- Public interface exposed to consumers (tests expect these methods on the returned table).
    return {
        push = push,
        pop = pop,
        shift = shift,
        unshift = unshift,
        count = count,
        delete = delete,
    }
end
