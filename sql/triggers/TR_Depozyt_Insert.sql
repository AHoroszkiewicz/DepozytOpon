CREATE OR ALTER TRIGGER TR_Depozyt_Insert
ON Depozyt
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Zapobiegamy rekurencji
    IF (TRIGGER_NESTLEVEL() > 1) RETURN

    UPDATE d
    SET 
        UtworzonoPrzez = ISNULL(CONVERT(nvarchar(256), SESSION_CONTEXT(N'CurrentUser')), SYSTEM_USER),
        DataEdycji = GETDATE(),
        EdytowanoPrzez = ISNULL(CONVERT(nvarchar(256), SESSION_CONTEXT(N'CurrentUser')), SYSTEM_USER)
    FROM Depozyt d
    INNER JOIN inserted i ON d.Id = i.Id;
END;
