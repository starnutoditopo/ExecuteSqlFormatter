# Easy way to convert exec sp_executesql to a normal query
A .Net tool to convert `exec sp_executesql` statements to a *normal* query.

This application pastes an `exec sp_executesql` statement from the clipboard, converts it to a *normal* query and copies the result to the clipboard.

**Example:**

Copy this:

```SQL
exec sp_executesql N'UPDATE MyTable SET [Field1] = @0, [Field2] = @1',N'@0 nvarchar(max) ,@1 int',@0=N'String',@1=0
```

to get this in the clipboard:

```SQL
UPDATE MyTable SET [Field1] = N'String', [Field2] = 0
```

## Credits

- https://stackoverflow.com/a/40403351/1288109
- https://github.com/MrM40/W-WinClipboard
