WEIGHT_OF_EMPTY_BOX equ 500
TRUCK_HEIGHT equ 300
PAY_PER_BOX equ 5
PAY_PER_TRUCK_TRIP equ 220

section .text

; the global directive makes a function visible to the test files
global get_box_weight
get_box_weight:
    ; This function takes the following parameters:
    ; - The number of items for the first product in the box, as a 16-bit non-negative integer
    ; - The weight of each item of the first product, in grams, as a 16-bit non-negative integer
    ; - The number of items for the second product in the box, as a 16-bit non-negative integer
    ; - The weight of each item of the second product, in grams, as a 16-bit non-negative integer
    ; The function must return the total weight of a box, in grams, as a 32-bit non-negative integer
    ; Calculate weight of first product
    movzx rax, di                    ; Load number of items for first product
    movzx rbx, si                    ; Load weight of each item of first product
    imul rax, rbx                    ; rax = number_of_items1 * weight_per_item1
    ; Calculate weight of second product
    movzx rbx, dx                    ; Load number of items for second product
    movzx rcx, cx                    ; Load weight of each item of second product
    imul rbx, rcx                    ; rbx = number_of_items2 * weight_per_item2
    ; Add weights of both products
    add rax, rbx                     ; rax = weight_product1 + weight_product2
    ; Add weight of empty box
    add rax, WEIGHT_OF_EMPTY_BOX     ; rax = total_weight + weight_of_empty_box
    ; Return total weight in rax
    ret

global max_number_of_boxes
max_number_of_boxes:
    ; This function takes the following parameter:
    ; - The height of the box, in centimeters, as a 8-bit non-negative integer
    ; The function must return how many boxes can be stacked vertically, as a 8-bit non-negative integer
    movzx rbx, di                    ; Load height of the box
    mov rax, TRUCK_HEIGHT            ; Load truck height
    xor rdx, rdx                     ; Clear upper half of dividend
    div rbx                          ; rax = TRUCK_HEIGHT / height_of_box
    ret

global items_to_be_moved
items_to_be_moved:
    ; This function takes the following parameters:
    ; - The number of items still unaccounted for a product, as a 32-bit non-negative integer
    ; - The number of items for the product in a box, as a 32-bit non-negative integer
    ; The function must return how many items remain to be moved, after counting those in the box, as a 32-bit integer
    mov eax, edi                     ; Load number of unaccounted items
    mov ebx, esi                     ; Load number of items in the box
    sub eax, ebx                     ; eax = unaccounted_items - items_in_box
    ret

global calculate_payment
calculate_payment:
    ; This function takes the following parameters:
    ; - The upfront payment, as a 64-bit non-negative integer
    ; - The total number of boxes moved, as a 32-bit non-negative integer
    ; - The number of truck trips made, as a 32-bit non-negative integer
    ; - The number of lost items, as a 32-bit non-negative integer
    ; - The value of each lost item, as a 64-bit non-negative integer
    ; - The number of other workers to split the payment/debt with you, as a 8-bit positive integer
    ; The function must return how much you should be paid, or pay, at the end, as a 64-bit integer (possibly negative)
    ; Remember that you get your share and also the remainder of the division
    ; Calculate earnings from boxes
    mov rax, rsi                     ; Load boxes moved
    mov rbx, PAY_PER_BOX             ; Load pay per box
    imul rax, rbx                    ; rax = boxes * PAY_PER_BOX
    ; Add income from truck trips
    mov rbx, rdx                     ; Load truck trips
    mov r11, PAY_PER_TRUCK_TRIP      ; Load pay per truck trip
    imul rbx, r11                    ; rbx = truck trips * PAY_PER_TRUCK_TRIP
    add rax, rbx                     ; add truck component
    ; Deduct the penalty for lost items
    mov rbx, rcx                     ; Load number of lost items
    mov r11, r8                      ; Load value of each lost item
    imul rbx, r11                    ; rbx = lost_items * value_per_lost_item
    sub rax, rbx                     ; subtract lost item penalty
    ; Remove the upfront payment already received
    sub rax, rdi                     ; subtract upfront payment
    ; Divide between you and the other workers, keeping the remainder
    movzx rbx, r9b                   ; Load number of other workers
    inc rbx                          ; rbx = other_workers + 1 (add yourself)
    cqo                              ; sign-extend rax into rdx:rax
    idiv rbx                         ; rax = total_payment / (other_workers + 1), rdx = remainder
    add rax, rdx                     ; rax = worker_share + remainder
    ret

%ifidn __OUTPUT_FORMAT__,elf64
section .note.GNU-stack noalloc noexec nowrite progbits
%endif
