using System;

namespace Model
{
    public record Character(Guid Id, char Char, long Version);
}