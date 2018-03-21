package hello;

import hello.dbCalls.QuestionsList;
import hello.dbCalls.Results;
import org.json.JSONArray;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.sql.ResultSet;
import java.sql.SQLException;

@RestController
public class MarkersController { //For markers section
    @RequestMapping(value = "/getAskedQuestions")
    public String getMyQuestions(@RequestParam(value = "id", required = true) int id, @RequestParam(value = "amount", required = false, defaultValue = "15") int amount, @RequestParam(value = "search", required = false, defaultValue = "") String search) {
        MsSqlConnection connection = new MsSqlConnection();

        ResultSet questions = QuestionsList.getUserAskedQuestions(connection.con, id, id, amount, true);

        JSONArray arr = new JSONArray();
        try {
            try {
                arr = ResultSetConverter.convert(questions);
            } catch (org.json.JSONException e) {
                e.printStackTrace();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return arr.toString();
    }

    @RequestMapping(value = "/getFavoriteQuestions")
    public String getFavoriteQuestions(@RequestParam(value = "id", required = true) int id, @RequestParam(value = "amount", required = false, defaultValue = "15") int amount, @RequestParam(value = "search", required = false, defaultValue = "") String search) {
        MsSqlConnection connection = new MsSqlConnection();

        ResultSet questions = QuestionsList.getUserFavoriteQuestions(connection.con, id, id, amount, true);

        JSONArray arr = new JSONArray();
        try {
            try {
                arr = ResultSetConverter.convert(questions);
            } catch (org.json.JSONException e) {
                e.printStackTrace();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return arr.toString();
    }

    @RequestMapping(value = "/getAnsweredQuestions")
    public String getHistoryQuestions(@RequestParam(value = "id", required = true) int id, @RequestParam(value = "amount", required = false, defaultValue = "15") int amount, @RequestParam(value = "search", required = false, defaultValue = "") String search) {
        MsSqlConnection connection = new MsSqlConnection();

        ResultSet questions = QuestionsList.getUserAnsweredQuestions(connection.con, id, id, amount, true);

        JSONArray arr = new JSONArray();
        try {
            try {
                arr = ResultSetConverter.convert(questions);
            } catch (org.json.JSONException e) {
                e.printStackTrace();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }

        return arr.toString();
    }
}
