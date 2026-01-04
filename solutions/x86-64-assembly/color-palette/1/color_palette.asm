default rel

section .rodata

global RED
RED dd 0xFF000000

global GREEN
GREEN dd 0x00FF0000

global BLUE
BLUE dd 0x0000FF00


section .data

global base_color
base_color dd 0xFFFFFF00

; Destination address for make_color_combination
make_color_combination_dest dq 0


section .text

extern combining_function

global get_color_value
get_color_value:
    mov eax, [rdi]
    ret

global add_base_color
add_base_color:
    mov eax, [rdi]
    mov [base_color], eax
    ret

global make_color_combination
make_color_combination:
    mov [make_color_combination_dest], rdi
    mov edi, [base_color]
    mov esi, [rsi]
    call combining_function
    mov rdx, [make_color_combination_dest]
    mov dword [rdx], eax
    ret

%ifidn __OUTPUT_FORMAT__,elf64
section .note.GNU-stack noalloc noexec nowrite progbits
%endif
