using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace TC_Logic_Gate
{
    // Reading csv.
    public class CsvStremReader
    {
        private ArrayList rowAL;
        private string fileName; //File Name

        private Encoding encoding; //Encoding

        public CsvStremReader()
        {
            rowAL = new ArrayList();
            fileName = string.Empty;
            encoding = Encoding.Default;
        }

        /// <param name="fileName">File name includes file path</param>
        public CsvStremReader(string fileName)
        { 
            rowAL = new ArrayList();
            this.fileName = fileName;
            encoding = Encoding.Default;
            LoadCsvFile();

        }
       
        /// <param name="fileName">File Name</param>
        /// <param name="encoding">File Encoding</param>
        public CsvStremReader(string fileName, Encoding encoding)
        {
            rowAL = new ArrayList();
            this.fileName = fileName;
            this.encoding = encoding;
            LoadCsvFile();
        }
        /// <summary>
        /// File name includes file path
        /// </summary>
        public string FileName
        {
            set
            {
                this.fileName = value;
                LoadCsvFile();
            }
        }
        /// <summary>
        /// File Encoding
        /// </summary>
        public Encoding FileEncoding
        {
            set
            {
                this.encoding = value;
            }
        }
        /// <summary>
        /// Getting the row number
        /// </summary>
        public int RowCount
        {
            get
            {
                return this.rowAL.Count;
            }
        }
        /// <summary>
        /// Getting the column number
        /// </summary>
        public int ColCount
        {
            get
            {
                int maxCol;
                maxCol = 0;
                for(int i = 0; i < this.rowAL.Count; i++)
                {
                    ArrayList colAL = (ArrayList)this.rowAL[i];
                    maxCol = (maxCol > colAL.Count) ? maxCol : colAL.Count;
                }
                return maxCol;
            }
        }
        /// <summary>
        /// Getting data of certain column
              /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string this[int row, int col]
        {
            get
            {
                //Data validity verification
                CheckRowValid(row);
                CheckColValid(col);
                ArrayList colAL = (ArrayList)this.rowAL[row - 1];
                // If the data in the requested column is greater than the column in the current row, return null
                if (colAL.Count < col)
                {
                    return "";
                }
                return colAL[col - 1].ToString();
            }
        }

        
        /// <summary>
        /// Checking if the row number is available.
        /// </summary>
        /// <param name="row"></param>
        private void CheckRowValid(int row)
        {
            if(row <= 0)
            {
                throw new Exception("The row number cannot be less than 0");
            }
            if(row > RowCount)
            {
                throw new Exception("No current data");
            }
        }

        private void CheckMaxRowValid(int maxRow)
        {
            if(maxRow <= 0 && maxRow != -1){
                throw new Exception("The row number cannot be equal to 0 or less than -1");
            }
            if(maxRow > RowCount)
            {
                throw new Exception("No current data");
            }
        }
        /// <summary>
        /// Checking if the column number is available.
        /// </summary>
        /// <param name="col"></param>
        private void CheckColValid(int col)
        {
            if(col <=0)
            {
                throw new Exception("The column number cannot be less than 0");
            }
            if(col > ColCount)
            {
                throw new Exception("No current data");
            }
        }
        /// <summary>
        /// Checking if the maximum column number is available.
        /// </summary>
        /// <param name="maxCol"></param>
        private void CheckMaxColValid(int maxCol)
        {
            if(maxCol <= 0 && maxCol != -1)
            {
                throw new Exception("The column number cannot be equal to 0 or less than -1");
            }
            if(maxCol > ColCount)
            {
                throw new Exception("No current data");
            }
        }

        /// <summary>
        /// Loading csv file
        /// </summary>
        private void LoadCsvFile()
        {
            //Data validity verification
            if (this.fileName == null)
            {
                throw new Exception("Please specify the name of the CSV file to be loaded");
            }
            else if (!File.Exists(this.fileName))
            {
                throw new Exception("The specified CSV file does not exist");
            }

            if(this.encoding == null)
            {
                this.encoding = Encoding.Default;
            }
            StreamReader streamReader = new StreamReader(fileName, encoding);
            string csvDataLine = string.Empty;

            while (true)
            {
                string fileDataLine;
                fileDataLine = streamReader.ReadLine();
                if(fileDataLine == null)
                {
                    break;
                }
                if(csvDataLine == "")
                {
                    csvDataLine = fileDataLine;
                }
                else
                {
                    csvDataLine += "\r\n" + fileDataLine;
                }

                //If it contains an even number of quotation marks, it means that there is a carriage return or a comma in the row of data
                if (!IfOddQuota(csvDataLine))
                {
                    AddNewDataLine(csvDataLine);
                    csvDataLine = "";
                }
            }
            streamReader.Close();
            //Odd number of quotation marks appear in the data line
            if (csvDataLine.Length > 0)
            {
                throw new Exception("The format of the CSV file is wrong");
            }
        }

        /// <summary>
        /// Determining whether the string contains an odd number of quotation marks
        /// </summary>
        /// <param name="dataLine">data row</param>
        /// <returns>is odd number，return true；or return false</returns>
        private bool IfOddQuota(string dataLine)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0; i < dataLine.Length; i++)
            {
                if (dataLine[i] == '\"')
                {
                    quotaCount++;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        /// <summary>
        /// Determine whether to start with an odd number of quotation marks

        /// </summary>
        /// <param name="dataCell"></param>
        /// <returns></returns>
        private bool IfOddStartQuota(string dataCell)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = 0; i < dataCell.Length; i++)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }

        /// <summary>
        /// Determine whether to end with an odd number of quotation marks
        /// </summary>
        /// <param name="dataCell"></param>
        /// <returns></returns>
        private bool IfOddEndQuota(string dataCell)
        {
            int quotaCount;
            bool oddQuota;

            quotaCount = 0;
            for (int i = dataCell.Length - 1; i >= 0; i--)
            {
                if (dataCell[i] == '\"')
                {
                    quotaCount++;
                }
                else
                {
                    break;
                }
            }

            oddQuota = false;
            if (quotaCount % 2 == 1)
            {
                oddQuota = true;
            }

            return oddQuota;
        }
        /// <summary>
        /// Add new data row

        /// </summary>
        /// <param name="newDataLine">new data row</param>
        private void AddNewDataLine(string newDataLine)
        {
            //System.Diagnostics.Debug.WriteLine("NewLine:" + newDataLine);

            ////return;

            ArrayList colAL = new ArrayList();
            string[] dataArray = newDataLine.Split(',');
            bool oddStartQuota;        //Whether to start with an odd number of quotation marks

            string cellData;

            oddStartQuota = false;
            cellData = "";
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (oddStartQuota)
                {
                    
                    cellData += "," + dataArray[i];
                    
                    if (IfOddEndQuota(dataArray[i]))
                    {
                        colAL.Add(GetHandleData(cellData));
                        oddStartQuota = false;
                        continue;
                    }
                }
                else
                {
                    //Whether to start with an odd number of quotation marks

                    if (IfOddStartQuota(dataArray[i]))
                    {
                        
                        if (IfOddEndQuota(dataArray[i]) && dataArray[i].Length > 2 && !IfOddQuota(dataArray[i]))
                        {
                            colAL.Add(GetHandleData(dataArray[i]));
                            oddStartQuota = false;
                            continue;
                        }
                        else
                        {

                            oddStartQuota = true;
                            cellData = dataArray[i];
                            continue;
                        }
                    }
                    else
                    {
                        colAL.Add(GetHandleData(dataArray[i]));
                    }
                }
            }
            if (oddStartQuota)
            {
                throw new Exception("There is a problem with the data format");
            }
            this.rowAL.Add(colAL);
        }
        /// <summary>
        /// Remove the opening and closing quotation marks of the box, and turn the double quotation marks into single quotation marks

        /// </summary>
        /// <param name="fileCellData"></param>
        /// <returns></returns>
        private string GetHandleData(string fileCellData)
        {
            if (fileCellData == "")
            {
                return "";
            }
            if (IfOddStartQuota(fileCellData))
            {
                if (IfOddEndQuota(fileCellData))
                {
                    return fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\""); 
                }
                else
                {
                    throw new Exception("Data quotes cannot be matched" + fileCellData);
                }
            }
            else
            {
               
                if (fileCellData.Length > 2 && fileCellData[0] == '\"')
                {
                    fileCellData = fileCellData.Substring(1, fileCellData.Length - 2).Replace("\"\"", "\""); 
                }
            }

            return fileCellData;
        }
    }
}
