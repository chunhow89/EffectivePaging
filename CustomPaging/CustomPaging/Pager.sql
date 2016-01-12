DELIMITER $$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetCustomers_Pager`(
	_PageIndex INT
   ,_PageSize INT
   ,OUT _RecordCount INT
)
BEGIN
	SET @RowNumber:=0;

	CREATE TEMPORARY TABLE Results
    SELECT  @RowNumber:=@RowNumber+1 RowNumber
		,custId
		,custName
		,custMobile
    FROM tblcustomer;

	SET _RecordCount = (SELECT COUNT(*) FROM Results);

	SELECT * FROM Results
    WHERE RowNumber BETWEEN (_PageIndex -1) * _PageSize + 1 AND (((_PageIndex -1) * _PageSize + 1) + _PageSize) - 1;

	DROP TEMPORARY TABLE Results;
END